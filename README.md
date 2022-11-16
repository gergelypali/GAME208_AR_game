# GAME208_AR_game
AR game for GAME208 - Card based game, where your goal is to reach 150 points first and you are competing against RandomCardDropper bots.

This is my very first AR game made with Unity and ARCore. Tested on Android phone.
Used features:
plane detection - visualize the detected planes with custom texture
ar raycast - interacting with the detected plane to place object in the virtual space
physics raycast - interacting the placed object so we can touch and place the choosen cards
singleton - gamemanager class to handle the global events and data
delegates/events - used to handle the different events during gameplay and to connect the scripts with a nice way
ConcurrentQueue - to create a shuffled deck and to handle the race condition when the game pulls new cards and these events can happen almost at the same time