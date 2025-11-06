
// https://medium.com/@diegogauna.developer/restful-api-using-typescript-and-react-hooks-3d99bdd0cd39
// https://nextjs.org/docs/pages/guides/environment-variables
import { useEffect, useState } from 'react'

type Project = {
	name: string
	description: string
	link: string
	media: string
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

// Media can be an image or video
export const PROJECTS: Project[] = [
	{
		name: 'Yurrgoht Game Engine',
		description: 'A Vulkan game engine developed using the Entity Component structure. Includes Scripting, GUI Editing, and reflection-based serialization.',
		link:  '#project-links',
		media: '/videos/yurrgoht_engine_demo.mp4',
		id: 'project1',
	},
	{
		name: 'Terrain Generator',
		description: 'Double precision procedural terrain generator utilizing geometry clipmaps ',
		link:  '#project-links',
		media: '/videos/terrain_generator_demo.gif',
		id: 'project2',
	},
	{
		name: 'Path-Trace-Demo-Rust',
		description: 'Accurate path-tracing algorithm, created in rust, generating standard resolution images.',
		link:  '#project-links',
		media: '/images/ray_trace_demo.png',
		id: 'project3',
	}
]

export const WORK_EXPERIENCE: WorkExperience[] = [
	{
		company: 'Bevy Foundation',
		title: 'Quality Assurance (Volunteer)',
		start: '2024',
		end: 'Present',
		link: 'https://bevyengine.org/foundation/',
		id: 'work1',
	}
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
		link: 'https://github.com/michaeljhkim',
	},
	{
		label: 'LinkedIn',
		link: 'https://www.linkedin.com/in/michael-jh-kim/',
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
	id: string;
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

export type HelloDescriptionData = {
	role: string;
	website_description: string;
	about_me: string;
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