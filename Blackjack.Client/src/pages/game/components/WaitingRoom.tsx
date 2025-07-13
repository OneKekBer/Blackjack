import React, { useState } from 'react'
import axios from 'axios'
import type { Game } from '../../../types/Game'

interface Props {
	game: Game
	userId: string
	startGame: () => void
	addBot: () => void
}

const WaitingRoom: React.FC<Props> = ({ game, userId, startGame, addBot }) => {
	const [newName, setNewName] = useState('')
	const [loading, setLoading] = useState(false)

	const player = game.players.find(p => p.userId === userId)

	const handleChangeName = async () => {
		if (!player || !newName.trim()) return

		try {
			setLoading(true)
			await axios.post(
				`${import.meta.env.VITE_API_URL}api/player/change-name`,
				{
					PlayerId: player.id,
					UserId: userId,
					NewName: newName.trim(),
				}
			)

			setNewName('')
		} catch (error) {
			console.error('Failed to change name:', error)
			alert('Error changing name')
		} finally {
			setLoading(false)
		}
	}

	return (
		<div>
			<h2 className='text-xl font-bold mb-4'>Game Id: {game.id}</h2>
			<h3 className='text-lg font-semibold'>Players</h3>
			<button
				onClick={addBot}
				className='mt-2 px-4 py-2 bg-purple-600 text-white rounded hover:bg-purple-700 transition'
			>
				Add Bot
			</button>
			<div className='space-y-2 mt-4'>
				{game.players.map((player, idx) => (
					<div key={idx} className='p-2 bg-gray-100 rounded shadow'>
						{player.name ?? player.id} ({player.connectionId})
					</div>
				))}
			</div>

			{player && (
				<div className='mt-6 space-x-2'>
					<input
						type='text'
						value={newName}
						onChange={e => setNewName(e.target.value)}
						placeholder='Enter your name'
						className='px-3 py-2 border rounded'
						disabled={loading}
					/>
					<button
						onClick={handleChangeName}
						className='px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 transition disabled:opacity-50'
						disabled={loading}
					>
						{loading ? 'Saving...' : 'Change Name'}
					</button>
				</div>
			)}

			<button
				onClick={startGame}
				className='mt-6 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition'
			>
				Start Game
			</button>
		</div>
	)
}

export default WaitingRoom
