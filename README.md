# EntityFrameworkTestApplication
The academic exercise/Just for fun/Just for lulz.

  - Probably it is necessary re-write caching techniques and only need one hits to Map and only one to Stogare. (If you add a hit to Stogare5 in this version, all it freezes).
  - ~~SO MANY ABSOLUTELY IDENTICAL CODE: At the time of writing, I did not know how to offer to solve this problem. Now I have an idea, but do not have time to check it. Most likely, you can come up with a single parent for the context of storages and then to cast them before use.~~ UPDATE (20.07.2016) I found a solution:https://gist.github.com/7BF794B0/ec0735cf4e19f8c9508d2cc73f6ebb67
  - The algorithm for creating connections between users (friendship) is very slow. I saw the algorithm that runs faster, but do not have time to rewrite :(
  - The method lstFriends_MouseDoubleClick in the Controller class units were try...catch. But they had to be removed because decreased speed (+2 seconds per request) and speed is important. But you can not live without treatment exceptions.
  - Logging system is also implemented is wrong, would probably now I have tried to use Nlog.

### List of used Nuget Packages:
  - EntityFramework
  - Newtonsoft.Json
  - SphinxConnector
  - StackExchange.Redis

### Date of creation of the project: 07.04.2015
### Date of big refactoring: 02.02.2016 - 04.02.2016
