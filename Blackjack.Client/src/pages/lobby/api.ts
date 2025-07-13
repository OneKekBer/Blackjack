import axios from 'axios'
import type { Game } from '../../types/Game'

export const CreateGame = async () => {
	try {
		const res = await axios.get(
			import.meta.env.VITE_API_URL + 'api/game/create'
		)
		console.log('Game created:', res.data)
	} catch (error) {
		console.error('Error creating game:', error)
	}
}

export const GetAllGames = async () => {
	try {
		const res = await axios.get<Game[]>(
			import.meta.env.VITE_API_URL + 'api/game/get-all'
		)
		console.log('Games:', res.data)
		return res.data
	} catch (error) {
		console.error('Error fetching games:', error)
	}
}
