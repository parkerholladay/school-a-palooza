#!/bin/bash

white="\033[0m"
green="\033[0;32m"
cyan="\033[0;36m"
yellow="\033[0;33m"
red="\033[0;31m"

run_spec="$NUNIT_PATH --nologo --noxml"
script_path=`dirname $0`
unit_specs=(
  "$script_path/../dotnet/SchoolAPalooza.Application.Specs/bin/Debug/SchoolAPalooza.Application.Specs.dll"
)
integration_specs=(
  "$script_path/../dotnet/SchoolAPalooza.Infrastructure.IntegrationSpecs/bin/Debug/SchoolAPalooza.Infrastructure.IntegrationSpecs.exe"
)

run_specs() {
  retVal=0
  start=`date +%s`

  if ! eval "$run_spec $@"; then
    echo -e "${red}Tests failed!${white}"
    retVal=1
  fi

  stop=`date +%s`
  echo -e "${yellow}Elapsed time: $((stop-start))s${white}"

  if [ $retVal != 0 ]; then
    return 1
  fi
}

if [[ ! -z "$1" && "$1" != "--unit" ]]; then
  echo -e "${yellow}Unkown argument ${cyan}$1${yellow}!${white}"
  exit 1
fi

echo -e "\n${green}Running ${cyan}unit${green} tests...${white}"
run_specs "${unit_specs[@]}"

if [ -z "$1" ]; then
  echo -e "\n${green}Running ${cyan}integration${green} tests...${white}"
  run_specs "${integration_specs[@]}"
fi

