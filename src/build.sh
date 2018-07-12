#!/bin/sh

docker-compose build

docker-compose up -d rabbitmq
docker-compose up -d postgres
docker-compose up

