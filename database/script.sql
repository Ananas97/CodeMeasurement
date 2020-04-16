 -- script which creates necessary tables in PostgreSQL
 
 DROP TABLE IF EXISTS account;
 DROP TABLE IF EXISTS project;
 DROP TABLE IF EXISTS project_source;
 DROP TABLE IF EXISTS general_metrics;
 DROP TABLE IF EXISTS class_metrics;
 DROP TABLE IF EXISTS method_metrics;
 
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
 project_id BIGINT REFERENCES project(project_id) NOT NULL,
 update_date TIMESTAMP NOT NULL,
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 number_of_namespaces BIGINT,
 number_of_classes BIGINT,
 PRIMARY KEY(project_id,update_date)
 );
 
 CREATE TABLE class_metrics(
 class_id BIGSERIAL PRIMARY KEY,
 class_name VARCHAR(100) NOT NULL,
 update_date TIMESTAMP NOT NULL,
 project_id BIGINT REFERENCES project(project_id),
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 number_of_childrens BIGINT,
 depth_of_inheritance_text BIGINT,
 weighted_methods BIGINT,
 UNIQUE(class_name,project_id,update_date)
 );
 
 CREATE TABLE method_metrics(
 method_name VARCHAR(200) NOT NULL,
 update_date TIMESTAMP NOT NULL,
 class_id BIGINT REFERENCES class_metrics(class_id) NOT NULL,
 lines_of_code BIGINT,
 lines_of_comments BIGINT,
 nested_block_depths BIGINT,
 PRIMARY KEY(method_name,update_date,class_id)
 );
 