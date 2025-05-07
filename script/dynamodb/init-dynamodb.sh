#!/bin/bash
echo "Esperando que LocalStack se inicie..." > /tmp/init.log
sleep 10
echo "Iniciando DynamoDB..." >> /tmp/init.log
sleep 10

set -e

echo "Creating Users table..."
awslocal dynamodb create-table \
  --table-name Users \
  --key-schema AttributeName=id,KeyType=HASH \
  --attribute-definitions AttributeName=id,AttributeType=S \
  --billing-mode PAY_PER_REQUEST

echo "Creating Vehicles table..."
awslocal dynamodb create-table \
  --table-name Vehicles \
  --key-schema AttributeName=Id,KeyType=HASH \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --billing-mode PAY_PER_REQUEST

echo "Creating ParkingLots table..."
awslocal dynamodb create-table \
  --table-name ParkingLots \
  --key-schema AttributeName=Id,KeyType=HASH \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --billing-mode PAY_PER_REQUEST

echo "Creating ParkingHistories table..."
awslocal dynamodb create-table \
  --table-name ParkingHistories \
  --key-schema AttributeName=Id,KeyType=HASH \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --billing-mode PAY_PER_REQUEST

echo "Creating RevokedTokens table..."
awslocal dynamodb create-table \
  --table-name RevokedTokens \
  --key-schema AttributeName=Id,KeyType=HASH \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --billing-mode PAY_PER_REQUEST

echo "All tables created successfully!"
