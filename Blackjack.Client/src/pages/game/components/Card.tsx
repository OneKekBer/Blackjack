// components/Card.tsx
import React from 'react'
import { rankMap, suitsMap } from '../../../types/Card'

type CardProps = {
	rank: number
	suits: number
	hidden?: boolean
}

const Card: React.FC<CardProps> = ({ rank, suits, hidden }) => {
	if (hidden) {
		return (
			<div className='w-12 h-16 bg-gray-400 border rounded shadow-inner flex items-center justify-center text-white'>
				💠
			</div>
		)
	}

	return (
		<div className='w-12 h-16 bg-white border rounded shadow text-center flex flex-col justify-center items-center text-lg'>
			<div>{rankMap[rank]}</div>
			<div className={suits === 1 || suits === 2 ? 'text-red-600' : ''}>
				{suitsMap[suits]}
			</div>
		</div>
	)
}

export default Card
