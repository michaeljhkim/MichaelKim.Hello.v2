'use client'
import { TextEffect } from '@/components/ui/text-effect'
import Link from 'next/link'
import {
	HelloDescriptionData,
	useFetchData
} from './data'

export function Header() {
	const HELLO_DESCRIPTION = useFetchData<HelloDescriptionData>("hello-descriptions");

	return (
		<header className="mb-8 flex items-center justify-between">
			<div>
				<Link href="/" className="font-medium text-black dark:text-white text-3xl">
					Michael Kim
				</Link>
				<div className="min-h-[1.5em]">
					{HELLO_DESCRIPTION.loading ? (
						<p className="text-zinc-500 animate-pulse">LOADING...</p>
					) : HELLO_DESCRIPTION.data?.role && (
						<TextEffect
							as="p"
							preset="fade"
							per="char"
							className="text-zinc-600 dark:text-zinc-500"
							delay={0.5}
						>
							{HELLO_DESCRIPTION.data.role}
						</TextEffect>
					)}
				</div>
			</div>
		</header>
	)
}