## Intro
Summary - Mini C++ console program that was an assignment for a class and contains lots of unrelated *crap* that was only for satisfying requirements.
Might be helpful for those, who are learning C++, because it does contain most of the C++ features and you can see it in action. That's my first C++ ever.

Originally - "A c++ desktop application for managing items in different warehouse, generating reports, etc."

## Some features
- Report generating
	- Asynchronous;
	- Fetching items soon to end;
	- Maintenance date;
	- Other information regarding items in a warehouse
- Code helpers (for those who are learning C++)
	- File writer;
	- Collection helper that encapsulates native C++ collections;
	- Timer helper makes simple time/date operations easier;
	
## Known issues
- Data layer is mocked (It should have been mysql database and some kind of encapsulation)
- No GUI
- High chance of bugs
- No tests (was deleted because of "reference/linking" maintainability nightmare
- Monolithe (one project)

## Prerequisites
- Project is built on **Visual Studio** 2015 Enterprise
- Requires **boost** library, configured in (project settings -> Configuration properties -> C/C++ -> General -> Additional Include Directories)
- Configurable through app.config

## Diagrams (might be slightly different from current code base):
![uml diagram](/../master/C++/UML.png)
![uml diagram](/../master/C++/GenerateReport_ActivityDiagram.png)
![uml diagram](/../master/C++/GenerateReport_SequenceDiagram.png)
![uml diagram](/../master/C++/UseCase.png)

## Documentation
Built on doxygen.
To recreate documentation, run this command in project root.
```
doxygen Documentation/Doxyfile
```

*Domas Vytautas Dziaugys s1310508, Vilnius University, Faculty of Mathematics and Informatics, 2016*
