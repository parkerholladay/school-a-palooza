#!/bin/bash

host="localhost"
adminUser="postgres"

psql -h $host -U $adminUser -c "select 1 from pg_database where datname = 'school_a_palooza';" | grep -q 1 && psql -h $host -U $adminUser -f ./"$(dirname $0)"/drop-local-db.sql
