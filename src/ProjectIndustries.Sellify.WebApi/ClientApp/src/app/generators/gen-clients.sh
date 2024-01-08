#!/usr/bin/env bash

mv "../sellify-api" "../sellify-api_tmp"
java -jar ./openapi-generator-cli-5.0.0.jar generate -i "http://localhost:5000/swagger/v1/swagger.json" -g typescript-angular -c codegen-clients-config.json -o "../sellify-api"
rm -r "../sellify-api_tmp"
