
// https://medium.com/@diegogauna.developer/restful-api-using-typescript-and-react-hooks-3d99bdd0cd39
// https://nextjs.org/docs/pages/guides/environment-variables
import { useEffect, useState } from 'react'

type Project = {
	name: string
	description: string
	link: string
	video: string
	id: string
}

type WorkExperience = {
	company: string
	title: string
	start: string
	end: string
	link: string
	id: string
}

type BlogPost = {
	title: string
	description: string
	link: string
	uid: string
}

type ProjectInfo = {
	title: string
	description: string
	link: string
	uid: string
}

type SocialLink = {
	label: string
	link: string
}

export const PROJECTS: Project[] = [
	{
		name: 'Motion Primitives Pro',
		description:
			'Advanced components and templates to craft beautiful websites.',
		link: 'https://pro.motion-primitives.com/',
		video:
			'https://res.cloudinary.com/read-cv/video/upload/t_v_b/v1/1/profileItems/W2azTw5BVbMXfj7F53G92hMVIn32/newProfileItem/d898be8a-7037-4c71-af0c-8997239b050d.mp4?_a=DATAdtAAZAA0',
		id: 'project1',
	},
	{
		name: 'Motion Primitives',
		description: 'UI kit to make beautiful, animated interfaces.',
		link: 'https://motion-primitives.com/',
		video:
			'https://res.cloudinary.com/read-cv/video/upload/t_v_b/v1/1/profileItems/W2azTw5BVbMXfj7F53G92hMVIn32/XSfIvT7BUWbPRXhrbLed/ee6871c9-8400-49d2-8be9-e32675eabf7e.mp4?_a=DATAdtAAZAA0',
		id: 'project2',
	},
]

export const WORK_EXPERIENCE: WorkExperience[] = [
	{
		company: 'Reglazed Studio',
		title: 'CEO',
		start: '2024',
		end: 'Present',
		link: 'https://ibelick.com',
		id: 'work1',
	},
	{
		company: 'Freelance',
		title: 'Design Engineer',
		start: '2022',
		end: '2024',
		link: 'https://ibelick.com',
		id: 'work2',
	},
	{
		company: 'Freelance',
		title: 'Front-end Developer',
		start: '2017',
		end: 'Present',
		link: 'https://ibelick.com',
		id: 'work3',
	},
]

export const BLOG_POSTS: BlogPost[] = [
	{
		title: 'Exploring the Intersection of Design, AI, and Design Engineering',
		description: 'How AI is changing the way we design',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-1',
	},
	{
		title: 'Why I left my job to start my own company',
		description:
			'A deep dive into my decision to leave my job and start my own company',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-2',
	},
	{
		title: 'What I learned from my first year of freelancing',
		description:
			'A look back at my first year of freelancing and what I learned',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-3',
	},
	{
		title: 'How to Export Metadata from MDX for Next.js SEO',
		description: 'A guide on exporting metadata from MDX files to leverage Next.js SEO features.',
		link: '/blog/example-mdx-metadata',
		uid: 'blog-4',
	},
]

export const SOCIAL_LINKS: SocialLink[] = [
	{
		label: 'Github',
		link: 'https://github.com/ibelick',
	},
	{
		label: 'Twitter',
		link: 'https://twitter.com/ibelick',
	},
	{
		label: 'LinkedIn',
		link: 'https://www.linkedin.com/in/ibelick',
	},
	{
		label: 'Instagram',
		link: 'https://www.instagram.com/ibelick',
	},
]

export const EMAIL = 'your@email.com'



export const PROJECT_INFO: ProjectInfo[] = [
	{
		title: 'MichaelKim.Hello Github',
		description: 'Source code for this Web Application',
		link: 'https://github.com/michaeljhkim/MichaelKim.Hello',
		uid: 'blog-1'
	}
]

export type PinnedRepo = {
	name: string;
	description: string;
	link: string;
	uid: string;
};

export type HelloInfoData = {
	first_name: string;
	last_name: string;
	age: number;
	email: string;
	github: string;
	linkedin: string;
	birth_date: string;
};

// can get PinnedRepos[], HelloInfoData, e.t.c
export function useFetchData<T>(endpointName: string) {
	const [data, setData] = useState<T | null>(null);
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState<Error | null>(null);

	useEffect(() => {
		fetch(`${process.env.NEXT_PUBLIC_HELLO_V2_API_URL}/${endpointName}`)
			.then((res) => {
				if (!res.ok) throw new Error("Network response was not ok");
				return res.json();
			})
			.then((json: T) => {
				console.log("Data from backend:", json);
				setData(json);
			})
			.catch((err) => {
				console.error("Error fetching data:", err);
				setError(err);
			})
			.finally(() => setLoading(false));
	}, [endpointName]);

	return { data, loading, error };
}