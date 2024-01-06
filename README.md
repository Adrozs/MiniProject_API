# Details
The goal was to create my first simple Webb-API. Using REST-architecturee to enable external services and apps to get and change data in the application.

## Assignment details and their endpoints

- **Get all people in the database**

  people/

- **Get all interest connected to a specific person**

  people/{personId}/interests/

- **Connect a person to a new interest**

  people/{personId}/interests/{interestId}

- **Add new links for a specific person and a specific interest**

  people/{personId}/interests/{interestId}/links

- **Get all links that are connected to a specific person**

  people/{personId}/interests/links

## EXTRA CHALLENGES
- **Give the option for the one calling the API and asking for a person to get out all interests and all links for that person directly in a hierarchical JSON-file**

  people/{personId}/hierarchical

- **Give the option for the one calling the API to filter what they get back, like a search. For example if we send "to" when getting all people in the database we should get back everyone that has a "to" in their name, like "Tobias or "Tomas". 
  This you can create for all calls (anrop) if you want.**

  - people/{search?}
  - people/page/{page?}/results/{results?}/{search?}
  - interests/{search?}
  - interests/page/{page?}/results/{results?}/{search?}

- **Create paginering of the calls. When we call for example people we maybe get the first 100 people and have to call more time to get more people. 
Here it could be nice that the call decides how many people we get in a call, so we can choose to get say 10 people if we just want that.**

  - people/page/{page?}/results/{results?}/{search?}
  - interests/page/{page?}/results/{results?}/{search?}


## Class Diagram
![UML-Diagram](https://github.com/adrozs/MiniProject_API/blob/master/UM_Diagram.png?raw=true)



## ER-Diagram
![ER-Diagram](https://github.com/adrozs/MiniProject_API/blob/master/ER_Diagram.png?raw=true)
