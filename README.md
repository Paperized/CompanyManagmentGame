# CompanyManagmentGame
A for fun managerial game in which you need to grow your software company!

Working in Unity6000 and GameObjects (not ECS).
I have decided to use events as the main source of communications between Components except of the core logic that needs to be done on Update (Money calculations etc...)

Currently:
- Made repository and standardized them with a base class and interfaces, managed automatically by the GameManager (Might put this on a RepositoryManager class depending on the size of the codebase)
- Random employee curriculum creation
- Hire or decline curriculum
- Random projects proposal creation
- Accept or decline project proposal
- Money calculator based on the amount of devs/analysts in the company
