 -- script which creates necessary tables in PostgreSQL
 
 -- create tables
 DROP TABLE IF EXISTS method_metrics;
 DROP TABLE IF EXISTS class_metrics;
 DROP TABLE IF EXISTS general_metrics;
 DROP TABLE IF EXISTS project;
 DROP TABLE IF EXISTS project_source;
 DROP TABLE IF EXISTS account;
  
 CREATE TABLE account(
 email VARCHAR(100) PRIMARY KEY,
 name VARCHAR(100) NOT NULL,
 password VARCHAR(100) NOT NULL
 );
 
 CREATE TABLE project_source(
 source_id INT IDENTITY(1,1) PRIMARY KEY,
 source_name VARCHAR(100) UNIQUE NOT NULL
 );
 
 CREATE TABLE project(
 project_id INT IDENTITY(1,1) PRIMARY KEY,
 name VARCHAR(250),
 creation_date DATETIME2 NOT NULL,
 last_update_date DATETIME2 NOT NULL,
 email VARCHAR(100) REFERENCES account(email),
 source_id INT REFERENCES project_source(source_id),
 UNIQUE(name,email)
 );
 
 CREATE TABLE general_metrics(
 general_metrics_id INT IDENTITY(1,1) PRIMARY KEY,
 project_id INT REFERENCES project(project_id),
 update_date DATETIME2 NOT NULL,
 lines_of_code INT,
 lines_of_comments INT,
 number_of_namespaces INT,
 number_of_classes INT,
 UNIQUE(project_id,update_date)
 );
 
 CREATE TABLE class_metrics(
 class_id INT IDENTITY(1,1) PRIMARY KEY,
 general_metrics_id INT REFERENCES general_metrics(general_metrics_id),
 class_name VARCHAR(100) NOT NULL,
 lines_of_code INT,
 lines_of_comments INT,
 number_of_childrens INT,
 depth_of_inheritance INT,
 weighted_methods INT,
 UNIQUE(class_name,general_metrics_id)
 );
 
 CREATE TABLE method_metrics(
 class_id INT REFERENCES class_metrics(class_id),
 method_name VARCHAR(200) NOT NULL,
 lines_of_code INT,
 lines_of_comments INT,
 nested_block_depths INT,
 PRIMARY KEY(method_name,class_id)
 );
 
 -- insert test data
 INSERT INTO account(email,name,password) VALUES
 ('test@gmail.com', 'testUser', 'test123');
 
 INSERT INTO project_source(source_name) VALUES
 ('github'),
 ('local source');
 
 INSERT INTO project(name, creation_date, last_update_date, email, source_id) VALUES
 ('test project', '2020-04-16 18:10:45', '2020-04-18 18:50:55', 'test@gmail.com', 1),
 ('another test project', '2020-04-17 12:10:45', '2020-04-17 12:10:45', 'test@gmail.com', 1);
 
 INSERT INTO general_metrics(project_id, update_date, lines_of_code, lines_of_comments, number_of_namespaces, number_of_classes) VALUES
 (1, '2020-04-16 18:10:45', 560, 60, 5, 1),
 (1, '2020-04-18 18:50:55', 1890, 150, 7, 2);
 
 INSERT INTO class_metrics(class_name, general_metrics_id, lines_of_code, lines_of_comments, number_of_childrens, depth_of_inheritance, weighted_methods) VALUES
 ('first class', 1, 560, 60, 0, 2, 3),
 ('first class', 2, 860, 80, 1, 3, 4),
 ('second class', 2, 1030, 70, 0, 2, 2);
 
 INSERT INTO method_metrics(method_name, class_id, lines_of_code, lines_of_comments, nested_block_depths) VALUES
 ('main', 1, 560, 60, 3),
 ('main', 2, 560, 60, 3),
 ('some function', 2, 300, 20, 1),
 ('print some values', 3, 1030, 70, 2);