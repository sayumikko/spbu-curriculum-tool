#!/bin/bash

getfiles(){
  cd WorkingPlans
	plans=()

  for file in *.docx; do
  plans+=("$file")
  done

  cd ..
}

execute(){
  cd Main
  for ((i = 0; i < ${#plans[@]}; i++))
  do
    echo "Учебный план ${plans[$i]:3:9}"
    dotnet run ${plans[$i]:3:9} -err; r=$?
    echo $r
  done
}

getfiles
execute

