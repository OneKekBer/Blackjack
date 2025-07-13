import type { Card } from './Card'

export enum PlayerAction {
	HIT = 0,
	STAND = 1,
}

export type Player = {
	id: string
	name: string | null
	balance: number
	cards: Card[]
	score: number
	isPlaying: boolean
	connectionId: string
	role: number
	userId: string
}
// id(pin):"e053a237-3a55-4789-8cf3-da636bb643ee"
// isPlaying(pin):true
// role(pin):0
// name(pin):null
// balance(pin):1000
// cards(pin):
// connectionId(pin):"m0sOY5Dhjhz3Mr-UfbyoWg"
// userId(pin):"a950b657-5789-4bab-ad53-9027c05ded8c"
