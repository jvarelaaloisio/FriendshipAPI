create table Friend_Request_States
(
	StateId smallint not null
		constraint Friend_Request_States_pk
			primary key nonclustered,
	description varchar(10)
)
go

create table Users
(
	UserId int identity
		constraint Users_pk
			primary key nonclustered,
	Name varchar(50) not null,
	Password varchar(50) not null
)
go

create table Friend_Requests
(
	RequestId bigint identity
		constraint Friend_Requests_pk
			primary key nonclustered,
	Sender int not null
		constraint Friend_Requests_Users_UserId_fk
			references Users
				on update cascade on delete cascade,
	Receiver int not null
		constraint Friend_Requests_Users_UserId_fk_2
			references Users,
	state smallint default 0 not null
		constraint Friend_Requests_Friend_Request_States_StateId_fk
			references Friend_Request_States
				on update cascade on delete cascade
)
go

create unique index Friend_Requests_RequestId_uindex
	on Friend_Requests (RequestId)
go

create index Friend_Requests_Receiver_index
	on Friend_Requests (Receiver)
go

create table Friendships
(
	UserId int not null
		constraint Users_Friendship_UserId
			references Users
				on update cascade on delete cascade,
	FriendId int not null
		constraint Users_Friendship_FriendId
			references Users,
	constraint user_friend
		primary key nonclustered (UserId, FriendId),
	constraint friend_user
		unique (FriendId, UserId)
)
go

create table Scores
(
	ScoreId bigint identity
		constraint Scores_pk
			primary key nonclustered,
	UserId int not null
		constraint Scores_Users_UserId_fk
			references Users
				on update cascade on delete cascade,
	ScoreValue int
)
go

create index Scores_ScoreValue_index
	on Scores (ScoreValue desc)
go

CREATE view Friend_Requests_With_Names as
        select FR.RequestId, FR.Sender, U1.Name as 'SenderName', FR.Receiver,U2.Name as 'ReceiverName', Fr.state, FRS.description as 'StateName'
        from Friend_Requests FR
                 inner join Users U1 on FR.Sender = U1.UserId
                 inner join Users U2 on FR.Receiver = U2.UserId
                 inner join Friend_Request_States FRS on FR.state = FRS.StateId
go

CREATE procedure AcceptFriendRequest(
    @RequestId bigint
)
as
begin
    set nocount on;
    declare @exists int;
    declare @UserId int;
    declare @FriendId int;
    set @exists = (
        select count(*)
        from Friend_Requests_With_Names
        where RequestId = @RequestId
          and StateName = 'pending'
    )
    if @exists < 1
        select 0;
    else
        begin
            update Friend_Requests
            set state = 1
            where RequestId = @RequestId;
            select 1;
            select @UserId = FR.Sender, @FriendId = FR.Receiver
                from Friend_Requests FR
            where RequestId = @RequestId;
            insert into Friendships (UserId, FriendId)
            values (@UserId, @FriendId);
        end
end;
go

create procedure DeclineFriendRequest(
    @RequestId bigint
)
as
begin
    set nocount on;
    declare @exists int;
    set @exists = (
        select count(*)
        from Friend_Requests_With_Names
        where RequestId = @RequestId
          and StateName = 'pending'
    )
    if @exists < 1
        select 0;
    else
        begin
            update Friend_Requests
            set state = 2
            where RequestId = @RequestId
            select 1;
        end
end;
go

CREATE procedure GenerateUser(
    @name varchar(50),
    @password varchar(50)
)
as
begin
    set nocount on;

    declare @quantity int;
    set @quantity =
            (select count(1)
             from Users
             where Name = @name
                 COLLATE Latin1_General_CS_AS);
    if @quantity > 0
            select 0;
    else
        begin
            insert into Users (name, password)
            values (@name, @password);
            select scope_identity();
        end
end
go

CREATE procedure GenerateUser2(
    @name varchar(50),
    @password varchar(50)
)
as
begin
    set nocount on;

    declare @exists int;
    exec @exists = ValidateUserExists @name;
    if @exists > 0
        select 0;
    else
        begin
            insert into Users (name, password)
            values (@name, @password);
            select scope_identity();
        end
end
go

CREATE procedure GetFriendRequests(
    @UserId int
)
as
begin
    select *
    from Friend_Requests_With_Names FRWN
    where FRWN.Receiver = @UserId
    and FRWN.state = 0
end;
go

CREATE procedure GetFriends(
    @UserId int
)
as
begin
    select U.UserId, U.Name from
    (select *
    from (select userid as 'friend'
          from Friendships
          where FriendId = @UserId) setOne
    UNION
    (select FriendId as 'friend'
    from Friendships
    where UserId = @UserId)) F
    inner join Users U on F.friend = U.UserId
end;
go

CREATE procedure GetTopScores(
    @quantity int
)
as begin
    select top (@quantity) u.Name, s.ScoreId, s.ScoreValue
    from Users u
    inner join Scores s on u.UserId = S.UserId
    order by ScoreValue desc
end
go

CREATE procedure GetUserId(
    @name varchar(50),
    @userId int OUTPUT
)
as
begin
    set nocount on;
    declare @exists int;
    exec @exists = ValidateUserExists @name;
    if @exists < 1
        select 0;
    else
        begin
            set @userId =
                (
                    select top 1 UserId
                    from Users
                    where Name COLLATE Latin1_General_CS_AS LIKE @name
                )
        end
end;
go

CREATE procedure RemoveFriend(
    @UserId int,
    @FriendId int
)
as
begin
    set nocount on;
    declare @friendshipExists BIT;
    exec @friendshipExists = ValidateFriendshipExists @UserId, @FriendId;
    if (@friendshipExists < 1)
        begin
            select 0;
            return 0;
        end
    else
        begin
            delete
            from Friendships
            where (
                    UserId = @UserId
                    and FriendId = @FriendId
                )
               or (
                    UserId = @FriendId
                    and FriendId = @UserId
                )
            delete
            from Friend_Requests
            where (Sender = @UserId
                and Receiver = @FriendId
                )
               or (
                    Sender = @FriendId
                    and Receiver = @UserId
                )
            select 1;
            return 1;
        end
end;
go

CREATE procedure SendFriendRequest(
    @UserId int,
    @FriendName varchar(50),
    @NewRequestId bigint output 
)
as
begin
    set nocount on;

    declare @FriendId int;
    declare @userExists BIT;
    declare @friendExists BIT;
    declare @userAndFriendAreEqual BIT;
    declare @friendshipExists BIT;
    declare @friendRequestExists BIT;

    exec @userExists = ValidateUserExistsById @UserId;
    exec GetUserId @FriendName, @FriendId out;
    exec @friendExists = ValidateUserExistsById @FriendId;
    set @userAndFriendAreEqual = ~ cast(abs(@UserId - @FriendId) as bit);
    exec @friendshipExists = ValidateFriendshipExists @UserId, @FriendId;
    exec @friendRequestExists = ValidatePendingFriendRequestExists @UserId, @FriendId;
    if (@userExists & @friendExists & ~@userAndFriendAreEqual & ~@friendshipExists & ~@friendRequestExists < 1)
        begin
            set @newRequestId = 0;
            return;
        end
    else
        begin
            begin try
                insert into Friend_Requests (Sender, Receiver)
                values (@UserId, @FriendId)
            end try
            begin catch
                set @newRequestId = 0;
                return;
            end catch
            set @newRequestId = scope_identity();
        end
end;
go

CREATE procedure ValidateFriendshipExists(
    @UserId int,
    @FriendId int
)
as
begin
    set nocount on;
    declare @exists bit;
    set @exists = (
        select count(1)
        from Friendships
        where (
                    UserId = @UserId
                and FriendId = @FriendId
            )
           or (
                    UserId = @FriendId
                and FriendId = @UserId
            )
    )
    return @exists;
end;
go

CREATE procedure ValidatePendingFriendRequestExists(
        @UserId int,
        @FriendId int
    )
    as
    begin
        set nocount on;
        declare @exists bit;
        set @exists = (
            select count(1)
            from Friend_Requests
            where (
                    (
                            Sender = @UserId
                            and Receiver = @FriendId
                        )
                    or (
                            Sender = @FriendId
                            and Receiver = @UserId
                        )
                )
              and state = 0
        )
        return @exists;
    end;
go

CREATE procedure ValidateUserExists(
    @name varchar(50)
)
as
begin
    set nocount on;
    declare @exists bit;
    set @exists =
            (select count(1)
             from Users
             where Name COLLATE Latin1_General_CS_AS like @name
            );
    return @exists;
end
go

CREATE procedure ValidateUserExistsById(
    @UserId int
)
as
begin
    set nocount on;
    declare @exists bit;
    set @exists =
            (select count(1)
             from Users
             where UserId = @UserId)
    return @exists;
end
go

create procedure ValidateUserPassword(
    @name varchar(50),
    @password varchar(50)
)
as
begin
    select count(1)
    from Users
    where Name = @name
        COLLATE Latin1_General_CS_AS
    and Password = @password
        COLLATE Latin1_General_CS_AS
end
go

-- we don't know how to generate root <with-no-name> (class Root) :(
use master
go

grant connect sql to db_a732bc_redesdb_admin
go

deny view any database to db_a732bc_redesdb_admin
go

use db_a732bc_redesdb
go

use master
go

grant view any database to [public]
go

use db_a732bc_redesdb
go

use master
go

grant connect sql to sa
go

use db_a732bc_redesdb
go

