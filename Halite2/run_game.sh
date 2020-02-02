#!/bin/bash
mcs hlt/Collision.cs hlt/Constants.cs hlt/DockMove.cs hlt/Entity.cs hlt/GameMap.cs hlt/Log.cs hlt/Metadata.cs hlt/MetadataParser.cs hlt/Move.cs hlt/Navigation.cs hlt/Networking.cs hlt/Planet.cs hlt/Player.cs hlt/Position.cs hlt/Ship.cs hlt/ThrustMove.cs hlt/UndockMove.cs hlt/Util.cs -out:MyBot.bin MyBot.cs
mcs hlt/Collision.cs hlt/Constants.cs hlt/DockMove.cs hlt/Entity.cs hlt/GameMap.cs hlt/Log.cs hlt/Metadata.cs hlt/MetadataParser.cs hlt/Move.cs hlt/Navigation.cs hlt/Networking.cs hlt/Planet.cs hlt/Player.cs hlt/Position.cs hlt/Ship.cs hlt/ThrustMove.cs hlt/UndockMove.cs hlt/Util.cs -out:Bot4.bin Bot4.cs
# mcs hlt/Collision.cs hlt/Constants.cs hlt/DockMove.cs hlt/Entity.cs hlt/GameMap.cs hlt/Log.cs hlt/Metadata.cs hlt/MetadataParser.cs hlt/Move.cs hlt/Navigation.cs hlt/Networking.cs hlt/Planet.cs hlt/Player.cs hlt/Position.cs hlt/Ship.cs hlt/ThrustMove.cs hlt/UndockMove.cs hlt/Util.cs -out:Bot3.bin Bot3.cs
# mcs hlt/Collision.cs hlt/Constants.cs hlt/DockMove.cs hlt/Entity.cs hlt/GameMap.cs hlt/Log.cs hlt/Metadata.cs hlt/MetadataParser.cs hlt/Move.cs hlt/Navigation.cs hlt/Networking.cs hlt/Planet.cs hlt/Player.cs hlt/Position.cs hlt/Ship.cs hlt/ThrustMove.cs hlt/UndockMove.cs hlt/Util.cs -out:Bot2.bin Bot2.cs
# mcs hlt/Collision.cs hlt/Constants.cs hlt/DockMove.cs hlt/Entity.cs hlt/GameMap.cs hlt/Log.cs hlt/Metadata.cs hlt/MetadataParser.cs hlt/Move.cs hlt/Navigation.cs hlt/Networking.cs hlt/Planet.cs hlt/Player.cs hlt/Position.cs hlt/Ship.cs hlt/ThrustMove.cs hlt/UndockMove.cs hlt/Util.cs -out:Bot1.bin Bot1.cs
# ./halite -i replays/ -d "360 240" "mono MyBot.bin Flagship" "mono Bot4.bin Bot4" "mono Bot4.bin Bot3" "mono Bot4.bin Bot2"
./halite -i replays/ "mono MyBot.bin Flagship" "mono Bot4.bin Bot4" #"mono Bot4.bin Bot3" "mono Bot4.bin Bot2"
rm *.log
