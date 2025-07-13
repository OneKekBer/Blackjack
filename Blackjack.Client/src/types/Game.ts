import type { Player } from './Player'

export type Game = {
	id: string
	players: Player[]
	currentTurn: string
	status: number
	deck: number
}

export type ViewGame = {
	id: string
}
