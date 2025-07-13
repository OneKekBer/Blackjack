import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useCookies } from 'react-cookie'
import {
	HubConnection,
	HubConnectionBuilder,
	LogLevel,
} from '@microsoft/signalr'
import type { Game } from '../../types/Game'
import { PlayerAction } from '../../types/Player'
import WaitingRoom from './components/WaitingRoom'
import PlayingRoom from './components/PlayingRoom'
import { toast } from 'react-toastify'
import type { Card } from '../../types/Card'

const GamePage: React.FC = () => {
	const { gameId } = useParams<{ gameId: string }>()
	const [cookies] = useCookies()
	const [connection, setConnection] = useState<HubConnection | null>(null)
	const [game, setGame] = useState<Game | null>(null)
	const navigate = useNavigate()

	const userId = cookies['user-id']

	useEffect(() => {
		console.log('use effect trigger!!!!!!!!!')
		if (userId && gameId) Connect()
	}, [userId, gameId])

	const Connect = async () => {
		try {
			const con = new HubConnectionBuilder()
				.withUrl(import.meta.env.VITE_API_URL + 'gameHub')
				.configureLogging(LogLevel.Information)
				.build()

			con.on('SendNewGame', (newGame: Game) => {
				setGame(newGame)
			})

			con.on('SendError', (message: string) => {
				toast.error(message, {})
				navigate('/')
			})

			con.on('SendResult', (_, message: string) => {
				console.log('sendresult')
				alert(message)
			})

			con.on(
				'SendPlayerCards',
				(_, playerId: string, cards: Card[], score: number) => {
					console.log('SendPlayerCards', score)
					setGame(prev =>
						prev
							? {
									...prev,
									players: prev.players.map(player =>
										player.id === playerId
											? { ...player, cards, score }
											: player
									),
							  }
							: prev
					)
				}
			)

			con.on('SendNewTurnId', (_, currentPlayerId: string) => {
				console.log('SendNewTurnId', currentPlayerId)
				setGame(prev =>
					prev ? { ...prev, currentTurn: currentPlayerId } : prev
				)
			})

			con.on('SendGameState', (updatedGame: Game) => {
				console.log('SendGameState', updatedGame)
				setGame(updatedGame)
			})

			con.onclose(() => {
				setConnection(null)
				navigate('/')
			})

			await con.start()

			await con.invoke('JoinGame', {
				UserId: userId,
				GameId: gameId,
			})

			setConnection(con)
		} catch (error) {
			console.error('SignalR error:', error)
		}
	}

	if (!connection || !game) return <div>Loading...</div>

	return game.status === 1 ? (
		<WaitingRoom
			game={game}
			userId={userId}
			startGame={() => connection.invoke('StartGame', { gameId })}
			addBot={() =>
				connection.invoke('AddBotToLobby', {
					GameId: game.id,
					PlayerId: game.players.find(p => p.userId === userId)?.id,
				})
			}
		/>
	) : (
		<PlayingRoom
			game={game}
			userId={userId}
			onHit={() =>
				connection.invoke('GetPlayerAction', {
					GameId: game.id,
					PlayerId: game.players.find(p => p.userId === userId)?.id,
					Action: PlayerAction.HIT,
				})
			}
			onStand={() =>
				connection.invoke('GetPlayerAction', {
					GameId: game.id,
					PlayerId: game.players.find(p => p.userId === userId)?.id,
					Action: PlayerAction.STAND,
				})
			}
		/>
	)
}

export default GamePage
