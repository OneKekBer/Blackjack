import React from 'react'
import type { Game } from '../../../types/Game'
import Card from './Card'

interface Props {
	game: Game
	userId: string
	onHit: () => void
	onStand: () => void
}

const PlayingRoom: React.FC<Props> = ({ game, userId, onHit, onStand }) => {
	const player = game.players.find(p => p.userId === userId)
	const isCurrent = player?.id === game.currentTurn

	return (
		<div className='p-6 max-w-5xl mx-auto'>
			{/* Other players */}
			<div className='mb-12'>
				<h3 className='text-xl font-bold mb-4'>Other Players</h3>
				<div className='grid grid-cols-1 sm:grid-cols-2 gap-6'>
					{game.players
						.filter(p => p.userId !== userId)
						.map((p, idx) => (
							<div
								key={idx}
								className={`p-4 rounded-xl border shadow ${
									p.id === game.currentTurn
										? 'border-yellow-500 bg-yellow-50'
										: 'bg-white'
								}`}
							>
								<p className='font-semibold text-lg'>
									{p.name ?? 'Anonymous'}{' '}
									{p.id === game.currentTurn && (
										<span className='text-yellow-600'>(playing)</span>
									)}
								</p>
								<p className='text-sm text-gray-500'>
									Balance: ${p.balance}
								</p>
								<div className='flex gap-2 mt-3'>
									{p.cards.map((_, i) => (
										<Card key={i} rank={0} suits={0} hidden />
									))}
								</div>
							</div>
						))}
				</div>
			</div>

			{/* Your hand */}
			<div className='mt-10'>
				<h3 className='text-xl font-bold mb-2'>Your Hand</h3>
				<p className='mb-1 text-gray-600'>Balance: ${player?.balance}</p>
				<div className='flex gap-3 mb-2'>
					{player?.cards.map((card, idx) => (
						<Card key={idx} rank={card.rank} suits={card.suits} />
					))}
				</div>
				<p className='text-lg font-medium'>Score: {player?.score}</p>
			</div>

			{/* Action buttons */}
			{isCurrent && (
				<div className='flex justify-center gap-6 mb-8'>
					<button
						onClick={onHit}
						className='px-6 py-3 text-lg font-semibold bg-green-700 text-white rounded-xl shadow hover:bg-green-800 transition'
					>
						Hit
					</button>
					<button
						onClick={onStand}
						className='px-6 py-3 text-lg font-semibold bg-red-700 text-white rounded-xl shadow hover:bg-red-800 transition'
					>
						Stand
					</button>
				</div>
			)}
		</div>
	)
}

export default PlayingRoom
