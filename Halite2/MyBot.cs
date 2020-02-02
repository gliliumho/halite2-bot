using Halite2.hlt;
using System.Collections.Generic;
using System;

namespace Halite2
{
    public class MyBot
    {

        private static void FuckShipUp(GameMap gameMap, Ship ship, Ship targetShip, List<Move> moveList)
        {
            ThrustMove newThrustMove = Navigation.NavigateShipTowardsTarget(gameMap, ship, targetShip, Constants.MAX_SPEED, true, 90, 0.2);
            if (newThrustMove != null)
            {
                moveList.Add(newThrustMove);
            }
        }

        private static void FuckPlanetUp(GameMap gameMap, Ship ship, Position targetPlanet, List<Move> moveList)
        {
            ThrustMove newThrustMove = Navigation.NavigateShipTowardsTarget(gameMap, ship, targetPlanet, Constants.MAX_SPEED, false, 90, 1);
            if (newThrustMove != null)
            {
                moveList.Add(newThrustMove);
            }
        }

        private static void CirclePlanet(GameMap gameMap, Ship ship, Planet targetPlanet, List<Move> moveList)
        {
            if (ship.GetDistanceTo(targetPlanet) < 10)
            {
                moveList.Add(new ThrustMove(ship, ship.OrientTowardsInDeg(targetPlanet) + 90, Constants.MAX_SPEED/2));
                return;
            }

            ThrustMove newThrustMove = Navigation.NavigateShipToDock(gameMap, ship, targetPlanet, Constants.MAX_SPEED *2/3);
            if (newThrustMove != null)
            {
                moveList.Add(newThrustMove);
            }
        }

        public static void Main(string[] args)
        {
            string name = args.Length > 0 ? args[0] : "Sharpie";

            Networking networking = new Networking();
            GameMap gameMap = networking.Initialize(name);
            List<Move> moveList = new List<Move>();
            int myPlanets = 0;
            int numScouts = 0;

            for (; ; )
            {
                moveList.Clear();
                gameMap.UpdateMap(Networking.ReadLineIntoMetadata());

                numScouts = 0;
                foreach (Ship ship in gameMap.GetMyPlayer().GetShips().Values){
                    if (ship.IsScout()){
                        numScouts++;
                    }
                }

                myPlanets = 0;
                foreach (Planet p in gameMap.GetAllPlanets().Values){
                    if (p.IsOwned() && p.GetOwner() == gameMap.GetMyPlayerId()){
                        myPlanets++;
                    }
                }

                foreach (Ship ship in gameMap.GetMyPlayer().GetShips().Values)
                {
                    if (ship.GetDockingStatus() != Ship.DockingStatus.Undocked)
                    {
                        continue;
                    }

                    // if (numScouts < 1 && !(ship.IsScout()) && (myPlanets > 2))
                    // {
                    //     ship.SetScout(true);
                    //     numScouts++;
                    //     foreach (Planet p in gameMap.GetAllPlanets().Values){
                    //         if ( p.IsOwned() && p.GetOwner() != gameMap.GetMyPlayerId() ){
                    //             ship.SetTargetId(p.GetId());
                    //             break;
                    //         }
                    //     }
                    // }
                    //
                    // if (ship.IsScout())
                    // {
                    //     Planet target = gameMap.GetPlanet(ship.GetTargetId());
                    //     if (target.IsOwned() && target.GetOwner() != gameMap.GetMyPlayerId()) {
                    //         CirclePlanet(gameMap, ship, target, moveList);
                    //         break;
                    //     }
                    //     // else
                    //     // {
                    //     //     ship.SetScout(false);
                    //     //     numScouts--;
                    //     //     if (ship.CanDock(target))
                    //     //     {
                    //     //         moveList.Add(new DockMove(ship, target));
                    //     //         break;
                    //     //     }
                    //     // }
                    // }


                    var sorted = new SortedDictionary<double, Entity>(gameMap.NearbyEntitiesByDistance(ship));
                    foreach (KeyValuePair<double, Entity> item in sorted)
                    {

                        double distance = item.Key;
                        if (item.Value.GetType()==typeof(Ship) && distance < 80)
                        {
                            Ship targetShip = (Ship)item.Value;
                            if ( targetShip.GetOwner() == gameMap.GetMyPlayerId() )
                            {
                                continue;
                            }
                            else
                            {
                                FuckShipUp(gameMap, ship, targetShip, moveList);
                                break;
                            }
                        }

                        if (item.Value.GetType() == typeof(Planet))
                        {
                            Planet planet = (Planet)item.Value;
                            if ( planet.IsOwned() && planet.GetOwner() != gameMap.GetMyPlayerId() )
                            {
                                bool noMorePlanet = true;
                                foreach (Planet p in gameMap.GetAllPlanets().Values){
                                    if (!p.IsOwned()){
                                        noMorePlanet = false;
                                        break;
                                    }
                                }
                                if (noMorePlanet){
                                    // FuckPlanetUp(gameMap, ship, planet, moveList);
                                    CirclePlanet(gameMap, ship, planet, moveList);
                                    break;
                                }
                                continue;
                            }
                            else if ( planet.IsOwned() && planet.IsFull() )
                            {
                                // Can add guards to defend
                                // Random rnd = new Random();
                                // if (rnd.Next(1,10) > 9){
                                //     CirclePlanet(gameMap, ship, planet, moveList);
                                //     break;
                                // }
                                continue;
                            }
                            else
                            {
                                //Dock and conquer planet
                                if (ship.CanDock(planet))
                                {
                                    moveList.Add(new DockMove(ship, planet));
                                    break;
                                }

                                ThrustMove newThrustMove = Navigation.NavigateShipToDock(gameMap, ship, planet, Constants.MAX_SPEED);
                                if (newThrustMove != null)
                                {
                                    moveList.Add(newThrustMove);
                                }
                                break;
                            }

                        }

                        if (item.Value.GetType()==typeof(Ship))
                        {
                            Ship targetShip = (Ship)item.Value;
                            if ( targetShip.GetOwner() == gameMap.GetMyPlayerId() )
                            {
                                continue;
                            }
                            else
                            {
                                FuckShipUp(gameMap, ship, targetShip, moveList);
                                break;
                            }
                        }




                    }
                    // foreach (Planet planet in gameMap.GetAllPlanets().Values)
                    // {
                    //     if (planet.IsOwned())
                    //     {
                    //         continue;
                    //     }
                    //
                    //     if (ship.CanDock(planet))
                    //     {
                    //         moveList.Add(new DockMove(ship, planet));
                    //         break;
                    //     }
                    //
                    //     ThrustMove newThrustMove = Navigation.NavigateShipToDock(gameMap, ship, planet, Constants.MAX_SPEED / 2);
                    //     if (newThrustMove != null)
                    //     {
                    //         moveList.Add(newThrustMove);
                    //     }
                    //
                    //     break;
                    // }
                }
                Networking.SendMoves(moveList);
            }
        }
    }
}
