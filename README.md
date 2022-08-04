# Grill Master 2022
## Implementing this task:
Summer is upon us, so we would like to make a quick buck by opening a grilling shop. We expect quite an influx of customers and we want to make the business fully autonomous by building a fleet of robotic workers capable of grilling 24/7. We pitched this idea to a few investors and they loved it, but they want us to come up with some metrics about this business idea before they are willing to invest. You are going to be leading the development effort necessary for some statistical analysis so that our marketing team can come up with the material for the next investor meeting.

Your task is to develop the following application:

Given a list of grillable items, develop:

- A grill optimizer API, capable of calculating the optimal arrangement of items on a grill for random grilling scenarios. This is going to be the backbone of our calculations.

- A client for the above mentioned API - a CLI that is able to fetch a random grilling menu off of a third party API, run the optimization logic and display the results in a terminal. Our marketing team will come up with graphics based on this visualization. You also need to make sure that our BI team can warehouse the results of these optimizations, so we need two potential formats: one has to be visual (e.g a grill arrangement drawn in console) and another one is json-based, for data warehousing.

The data that you get from a third party API is a single random test case, containing information about the dimensions of the grill and various grillable items and their respective dimensions.
The optimizer API has to be able to find the optimal arrangement of items on a grill so that we are able to grill the full menu in the least amount of grill runs. For now we assume that all the items are taking the same amount of time to grill (we care about the volume more than the taste after all, since we are going to be serving drinks at the venue as well).

## A few remarks and limitations
- menu generator API is live at the following url:
https://grill-master-svc.herokuapp.com/api/menus
- when deciding on optimal grill arrangement, it is possible to rotate items to fit more things on the grill
- the algorithm does not have to be optimal 100% of the time, but has to be near optimal in the majority of cases
- Both the API and CLI have to be based on dotnet 6 - we already have an in-house team of .NET developers
- The API access has to be protected from unauthorized use.
- The core logic of the optimization process has to be unit-tested
- No API secrets must be stored in git.

## Bonus points
- Integration testing of the API and/or CLI
- API endpoint doc comments or a swagger endpoint with example usages
- Logging in the core application part detailing the process of optimization
- Proper development history in git with meaningful commit messages and/or branching
- Github actions deployment process into a free-tier Azure web application
