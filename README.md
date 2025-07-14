# Its my pet-project mulitplayer PVPVB blackjack 
Technology:
Backend: C#, Asp.Net, Signalr Entity framework, PostgreSQL, InMemmoryCache  
Frontend: React, Typescript, Tailwind

# Startup
If you dont have docker: Downolad docker! \
If you dont have postgres image: type in cmd ```docker pull postgres```
1) ```git clone https://github.com/OneKekBer/Blackjack.git```
2) in Blackjack dir cmd type: ```docker compose up -d --build```
3) go to ```http://localhost:8000```
4) enjoy

## What problems did i solve ?
1) For me the hardest thing was structure of this project, I wanted create some blackjack core that will easylly connect to other applications, for example web or console
2) Entity models, i wanted divide my objects to some logical parts. Entity for storing in database, Models for communictaion business layer and game core, View for displaying info to users. The hard part that i wrote my own mappers, and sometimes i had a problem with entity framework tracking and i shoulded to read and know about how entity framework works.
3) Spliting player ConnectionId into separate entity   
4) Queue system. In the start of my project queue was like index which iterates throught list of players. But this was buggy.
5) I realize that i dont need reids and i implemented inmemmory cahce by wrapping old repository with cached one

not ai generated)


