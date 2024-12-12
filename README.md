# Course Service

## Description
Course service provides the API for Courses, Modules, Topics, and Exercises (NOT questions)

## How to clone
Clone this repository AND ITS SUBMODULES by running:
```sh
git clone --recurse-submodules https://github.com/Hackleberry-group/course-service.git 
```

Too late? Cloned without submodules? Run the following command in the repository root:
```sh
git submodule update --init --recursive
```

## Port registered:
1030


## How to run Grafana and Prometheus, RabbitMQ, and Azure Table Storage

Run the following command in the repository CourseServiceAPI to run prometheus and grafana
```sh
docker run -d --name prometheus -p 9090:9090 -v ${PWD}/prometheus.yml:/etc/prometheus/prometheus.yml prom/prometheus
```
    
 To run the grafana   
```sh
docker run -d --name grafana -p 3000:3000 grafana/grafana
```

RabbitMQ should be running in the local machine
```sh
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Azure Table Storage should be running in the local machine