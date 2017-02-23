#!/bin/bash

white="\033[0m"
green="\033[0;32m"
cyan="\033[0;36m"

if [ -z "$1" ]; then
  echo "Must provide server host"
  exit 1
fi

if [ -z "$2" ]; then
  echo "Must provide user name"
  exit 1
fi

sql="./schema.sql"
db="school_a_palooza"
db_int="${db}_integration"

for file in `ls "$(dirname $0)"/???-*.sql | sort`; do
  cat $file >> $sql
done

echo -e "\n${green}Running ${cyan}db schema scripts${green} on ${db} db...${white}"
psql -h $1 -U $2 -d $db -f $sql
echo ""

if [ -z "$3" ] || [ "$3" != "prod" ]; then
  echo -e "\n${green}Running ${cyan}db schema scripts${green} on ${db_int} db...${white}"
  psql -h $1 -U $2 -d $db_int -f`` $sql
  echo ""
fi

rm $sql
