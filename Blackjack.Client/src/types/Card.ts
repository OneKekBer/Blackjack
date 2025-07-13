export type Card = {
	suits: number
	rank: number
}

export const suitsMap: Record<number, string> = {
	0: '♣', // Club
	1: '♦', // Diamond
	2: '♥', // Heart
	3: '♠', // Spade
}

export const rankMap: Record<number, string> = {
	0: 'A',
	1: 'K',
	2: 'Q',
	3: 'J',
	4: '10',
	5: '9',
	6: '8',
	7: '7',
	8: '6',
}
