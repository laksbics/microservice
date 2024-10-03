# Architecture Overview
![image](https://github.com/user-attachments/assets/458cf14e-3473-40c6-aba0-84834fe6a4b2)







## Components Used

- Docker
- Appache Kafka -- runs in docker
- Mongo Db -- runs in Docker
- MsSql -- runs in Docker


![image](https://github.com/user-attachments/assets/5fc70a37-da1b-40d2-aa3f-4fe2d67d65cb)



### Used docker desktop to deploy the microservices for real time data processing
![image](https://github.com/user-attachments/assets/e8e03087-3bd9-4577-902f-94350c43c63c)



## Functionalities

Handle commands and raise events.

Use the mediator pattern to implement command and query dispatchers.

Create and change the state of an aggregate with event messages.

Implement an event store / write database in MongoDB.

Create a read database in MS SQL.

Apply event versioning.

Implement optimistic concurrency control.

Produce events to Apache Kafka.

Consume events from Apache Kafka to populate and alter records in the read database.

Replay the event store and recreate the state of the aggregate.

Separate read and write concerns.

Structure your code using Domain-Driven-Design best practices.

Replay the event store to recreate the entire read database.

Replay the event store to recreate the entire read database into a different database type - PostgreSQL.

The ultimate goal of this course is to take a deep-dive into the world of CQRS and Event Sourcing to enable you to create microservices that are super decoupled and extremely scalable.
