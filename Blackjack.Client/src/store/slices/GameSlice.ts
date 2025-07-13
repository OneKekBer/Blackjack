import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'
import type { RootState } from '../Store'
import type { HubConnection } from '@microsoft/signalr'
import type { Game } from '../../types/Game'

// Define a type for the slice state
interface GameState {
	games: Game[]
	connectionId: HubConnection | null
}

// Define the initial state using that type
const initialState: GameState = {
	games: [],
	connectionId: null,
}

export const gamesSlice = createSlice({
	name: 'games',
	initialState,
	reducers: {
		AddGame: (state, action: PayloadAction<Game>) => {
			const existingGame = state.games.find(
				(game: Game) => game.id === action.payload.id
			)
			if (!existingGame) {
				state.games.push(action.payload)
			}
		},
		SetConnectionId(state, action: PayloadAction<HubConnection>) {
			state.connectionId = action.payload
		},
		UpdateGame: (state, action: PayloadAction<Game>) => {
			const index = state.games.findIndex(
				(game: Game) => game.id === action.payload.id
			)
			if (index !== -1) {
				state.games[index] = action.payload
			}
		},
	},
})

// Selector to find a game by its ID
export const GetGameById = (state: RootState, id: string) =>
	state.games.games.find(game => game.id === id)

export const { AddGame, UpdateGame, SetConnectionId } = gamesSlice.actions

export default gamesSlice.reducer
