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
    dotnet run ${plans[$i]:3:9} -err -nout; r=$?
    if (($r != 0)); then
      return $r
    fi
    echo "Проверка завершена успешно."
  done
}

getfiles
execute