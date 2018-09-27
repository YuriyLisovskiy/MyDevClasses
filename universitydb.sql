create table sessions (
  id          serial    primary key,
  semester    smallint  not null,
  date_start  date      not null,
  date_end    date      not null
);

create table lecturers (
  name        varchar(255)  primary key,
  department  varchar(255)  not null
);

create table sessions_lecturers (
  lecturer_name   varchar(255)  references lecturers(name),
  session_id      int           references sessions(id)
);

create table results (
  id          serial    primary key,
  mark        smallint  not null,
  is_failed   boolean   default false,
  session_id  int       not null references sessions(id) on delete cascade
);

create table students (
  name          varchar(255)  primary key,
  is_expelled   boolean       default false
);

create table subjects (
  id          serial        primary key,
  title       varchar(255)  not null,
  classroom   smallint      not null
);

create table exams (
  id              serial        primary key,
  timestamp       timestamp     not null,
  classroom       smallint      not null,
  subject_id      int           not null      references subjects(id),
  result_id       int           null          references results(id),
  lecturer_name   varchar(255)  not null      references lecturers(name),
  session_id      int           not null      references sessions(id)     on delete cascade,
  student_name    varchar(255)  not null      references students(name)
);

alter table results
    add exam_id int not null references exams(id) on delete cascade default 0;

alter table subjects
    add exam_id int null references exams(id);
