Its black jack made in c# and asp.net 
Stack: Entity framework, 

What problems did i solve ?
1) For me the hardest thing was structure of this project, I wanted create some blackjack core that will easylly connect to other applications, for example web or console
2) Entity models, i wanted divide my objects to some logical parts. Entity for storing in database, Models for communictaion business layer and game core, View for displaying info to users. The hard part that i wrote my own mappers, and sometimes i had a problem with entity framework tracking and i shoulded to read and know about how entity framework works
3) Queue system. In the start of my project queue was like index which iterates throught list of players. But this was buggy.
4) Write about redis caching when i will finish it
5) Performance upgrades 
