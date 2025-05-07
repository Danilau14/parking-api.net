#!/bin/sh
sleep 10

echo "Initializing localstack s3"

awslocal s3 mb s3://parking-api