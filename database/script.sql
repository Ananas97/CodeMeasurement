 -- script which creates necessary tables in PostgreSQL
 
 -- create tables
 DROP TABLE IF EXISTS account CASCADE;
 DROP TABLE IF EXISTS project CASCADE;
 DROP TABLE IF EXISTS project_source CASCADE;
 DROP TABLE IF EXISTS general_metrics CASCADE;
 DROP TABLE IF EXISTS class_metrics CASCADE;
 DROP TABLE IF EXISTS method_metrics CASCADE;
 
 CREATE TABLE account(
 email VARCHAR(100) PRIMARY KEY,
 name VARCHAR(100) NOT NULL,
 password VARCHAR(100) NOT NULL
 );
 
 CREATE TABLE project_source(
 source_id BIGSERIAL PRIMARY KEY,
 source_name VARCHAR(100) UNIQUE NOT NULL
 );
 
 CREATE TABLE project(
 project_id BIGSERIAL PRIMARY KEY,
 name VARCHAR(250),
 creation_date TIMESTAMP NOT NULL,
 last_update_date TIMESTAMP NOT NULL,
 email VARCHAR(100) REFERENCES account(email) NOT NULL,
 source_id BIGINT REFERENCES project_source(source_id),
 UNIQUE(name,email)
 );
 
 CREATE TABLE general_metrics(
 general_metrics_id BIGSERIAL PRIMARY KEY,
 project_id BIGINT REFERENCES project(project_id) NOT NULL,
 update_date TIMESTAMP NOT NULL,
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 number_of_namespaces BIGINT,
 number_of_classes BIGINT,
 UNIQUE(project_id,update_date)
 );
 
 CREATE TABLE class_metrics(
 class_id BIGSERIAL PRIMARY KEY,
 general_metrics_id BIGINT REFERENCES general_metrics(general_metrics_id) NOT NULL,
 class_name VARCHAR(100) NOT NULL,
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 number_of_childrens BIGINT,
 depth_of_inheritance BIGINT,
 weighted_methods BIGINT,
 UNIQUE(class_name,general_metrics_id)
 );
 
 CREATE TABLE method_metrics(
 class_id BIGINT REFERENCES class_metrics(class_id) NOT NULL,
 method_name VARCHAR(200) NOT NULL,
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 nested_block_depths BIGINT,
 PRIMARY KEY(method_name,class_id)
 );
 
 -- insert test data
 INSERT INTO account(email,name,password) VALUES
 ('test@gmail.com', 'testUser', 'test123');
 
 INSERT INTO project_source(source_id,source_name) VALUES
 (DEFAULT, 'github'),
 (DEFAULT, 'local source');
 
 INSERT INTO project(project_id, name, creation_date, last_update_date, email, source_id) VALUES
 (DEFAULT, 'test project', '2020-04-16 18:10:45', '2020-04-18 18:50:55', 'test@gmail.com', 1),
 (DEFAULT, 'another test project', '2020-04-17 12:10:45', '2020-04-17 12:10:45', 'test@gmail.com', 1);
 
 INSERT INTO general_metrics(general_metrics_id,project_id, update_date, lines_of_code, lines_of_comments, number_of_namespaces, number_of_classes) VALUES
 (DEFAULT, 1, '2020-04-16 18:10:45', 560, 60, 5, 1),
 (DEFAULT, 1, '2020-04-18 18:50:55', 1890, 150, 7, 2);
 
 INSERT INTO class_metrics(class_id, class_name, general_metrics_id, lines_of_code, lines_of_comments, number_of_childrens, depth_of_inheritance, weighted_methods) VALUES
 (DEFAULT, 'first class', 1, 560, 60, 0, 2, 3),
 (DEFAULT, 'first class', 2, 860, 80, 1, 3, 4),
 (DEFAULT, 'second class', 2, 1030, 70, 0, 2, 2);
 
 INSERT INTO method_metrics(method_name, class_id, lines_of_code, lines_of_comments, nested_block_depths) VALUES
 ('main', 1, 560, 60, 3),
 ('main', 2, 560, 60, 3),
 ('some function', 2, 300, 20, 1),
 ('print some values', 3, 1030, 70, 2);