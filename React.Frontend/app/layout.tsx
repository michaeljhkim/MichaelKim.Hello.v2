import type { Metadata, Viewport } from 'next'
import { Geist, Geist_Mono } from 'next/font/google'
import './globals.css'
import { Header } from './header'
import { Footer } from './footer'
import { ThemeProvider } from 'next-themes'

export const viewport: Viewport = {
	width: 'device-width',
	initialScale: 1,
	themeColor: '#ffffff',
}

export const metadata: Metadata = {
	title: 'MichaelKim.Hello.v2',
	description: 'Frontend created with Nim - https://github.com/ibelick/nim',
}

const geist = Geist({
	variable: '--font-geist',
	subsets: ['latin'],
})

const geistMono = Geist_Mono({
	variable: '--font-geist-mono',
	subsets: ['latin'],
})

export default function RootLayout( {children}: Readonly <{children: React.ReactNode}>) {
	return (
		<html lang="en" suppressHydrationWarning>
			<body className={`${geist.variable} ${geistMono.variable} bg-gradient-to-b from-black via-gray-800 to-white bg-[position:0_-200px] tracking-tight antialiased dark:from-black dark:via-zinc-850 dark:to-zinc-950`}>
			<ThemeProvider enableSystem={true} attribute="class" storageKey="theme" defaultTheme="dark">
				<div className="flex min-h-screen w-full flex-col font-[family-name:var(--font-inter-tight)]">
				<div className="relative mx-auto w-full max-w-screen-xl flex-1 px-4 pt-20">
					<Header />
					{children}
					<Footer />
				</div>
				</div>
			</ThemeProvider>
			</body>
		</html>
	)
}