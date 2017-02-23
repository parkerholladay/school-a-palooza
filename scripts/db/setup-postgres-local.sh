#!/bin/bash

host="localhost"
adminUser="postgres"

psql -h $host -U $adminUser -c "select 1 from pg_database where datname = 'school_a_palooza';" | grep -q 1 || psql -h $host -U $adminUser -f ./"$(dirname $0)"/create-local-db.sql
/bin/bash "$(dirname $0)"/run-schema-scripts.sh $host $adminUser
/bin/bash "$(dirname $0)"/seed-data/run-seed-scripts.sh $host $adminUser
