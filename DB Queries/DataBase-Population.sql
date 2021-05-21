------------------------------------------ USERS -----------------------------------------
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (5, N'Juampy', N'juampypass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (6, N'Yisu', N'yisupass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (7, N'Tate', N'tatepass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (8, N'Luigi', N'luigipass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (44, N'Gabriela', N'gabrielapass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (37, N'Mateo', N'mateopass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (45, N'Jorge', N'jorgepass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (49, N'Sandra', N'sandrapass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (38, N'Camila', N'camilapass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (50, N'Matias', N'matiaspass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (36, N'Federico', N'federicopass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (39, N'Ivan', N'ivanpass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (40, N'Tomas', N'tomaspass');
INSERT INTO db_a732bc_redesdb.dbo.Users (UserId, Name, Password) VALUES (41, N'Marcelo', N'marcelopass');
------------------------------------- REQUEST STATES ------------------------------------
INSERT INTO db_a732bc_redesdb.dbo.Friend_Request_States (StateId, description) VALUES (0, N'pending');
INSERT INTO db_a732bc_redesdb.dbo.Friend_Request_States (StateId, description) VALUES (1, N'accepted');
INSERT INTO db_a732bc_redesdb.dbo.Friend_Request_States (StateId, description) VALUES (2, N'declined');
------------------------------------- FRIEND REQUESTS ------------------------------------
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (66, 5, 49, 2);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (68, 5, 45, 2);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (73, 5, 38, 0);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (75, 5, 8, 0);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (95, 5, 49, 2);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (96, 49, 45, 1);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (10, 7, 5, 1);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (7, 7, 6, 1);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (99, 5, 6, 1);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (78, 49, 5, 2);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (102, 5, 37, 1);
INSERT INTO db_a732bc_redesdb.dbo.Friend_Requests (RequestId, Sender, Receiver, state) VALUES (103, 45, 5, 0);
------------------------------------- FRIENDSHIPS ------------------------------------
INSERT INTO db_a732bc_redesdb.dbo.Friendships (UserId, FriendId) VALUES (5, 6);
INSERT INTO db_a732bc_redesdb.dbo.Friendships (UserId, FriendId) VALUES (5, 37);
INSERT INTO db_a732bc_redesdb.dbo.Friendships (UserId, FriendId) VALUES (7, 5);
INSERT INTO db_a732bc_redesdb.dbo.Friendships (UserId, FriendId) VALUES (7, 6);
INSERT INTO db_a732bc_redesdb.dbo.Friendships (UserId, FriendId) VALUES (49, 45);
------------------------------------- SCORES ------------------------------------
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (1, 5, 100);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (2, 5, 1000);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (3, 5, 400);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (4, 5, 1000);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (5, 6, 2500);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (6, 6, 500);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (7, 6, 600);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (8, 6, 4500);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (9, 7, 1000);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (10, 7, 1600);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (11, 7, 3500);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (12, 8, 1200);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (13, 8, 1700);
INSERT INTO db_a732bc_redesdb.dbo.Scores (ScoreId, UserId, ScoreValue) VALUES (14, 8, 3100);