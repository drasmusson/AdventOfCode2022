# AdventOfCode2022

## Structure
For every day in the advent calendar I add a new project to the solution and a new .txt to the input project.

The project should be named 'Day01' for the first of December, 'Day11' for the eleventh of December and so on.

## Add new day
To add a project with the name Day01 run the following commands from the root of the repo:

Create a new console project:
`
dotnet new console -o Day01
`

Add the new project to the solution:
`
dotnet sln AdventOfCode2022.sln add Day01\Day01.csproj
`

Add a reference from the new project to the input project:
`
dotnet add Day01/Day01.csproj reference Input/Input.csproj
`
