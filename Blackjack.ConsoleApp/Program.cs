using Blackjack.ConsoleApp;
using Blackjack.ConsoleApp.MemmoryStore;
using Blackjack.ConsoleApp.Services;
using Blackjack.GameLogic;

var store = new Store();
var gameEngine = new GameEngine(
    new InputService(),
    new OutputService(),
    new GamePersisterService(store)
);
var gameRunner = new GameRunner(gameEngine, store);

await gameRunner.StartGameAsync();