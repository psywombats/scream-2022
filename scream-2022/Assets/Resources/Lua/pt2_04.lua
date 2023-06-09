if not getSwitch('pt2_04') then
	enterNVL()
	enter('NOEMI', 'c')
	speak("ARIEL", "Hey, Noemi? You here? It's Ariel.")
	speak("NOEMI", "Oh. It's Ariel.")
	speak("ARIEL", "Sorry. I know it's just me that barges into your lab every day.")
	speak("NOEMI", "No, no that's fine. I like it when you're here")
	speak("NOEMI", "Hm hm. You don't sound good, though.")
	speak("ARIEL", "And I don't feel good. What happened to me yesterday?")
	speak("NOEMI", "Yesterday?")
	speak("ARIEL", "Late last night, there was... Never mind, I can't describe it.")
	speak("ARIEL", "But I can't remember anything that happened today. There were lawyers here?")
	speak("NOEMI", "Hm hm. There were lots of visitors. I wouldn't know if they were lawyers because I was in the lab the whole time. There was a siren, but I think I dreamed that.")
	speak("NOEMI", "Maybe you should ask Chris?")
	speak("ARIEL", "Braulio said he was on leave. I talked with him a bit last night, and I'm not sure I should be the one saying it, but...")
	speak("ARIEL", "He said he might leave Lucir. And he said if Sumi funded us, that might be more likely.")
	speak("NOEMI", "I'm worried.")
	speak("ARIEL", "About Chris?")
	exit('NOEMI')
	enter('NOEMI', 'b')
	speak("NOEMI", "I met him in the dream world, and he seemed... very sad.")
	speak("ARIEL", "The last time I remember talking to him, he sounded on the verge of a breakthrough.")
	speak("NOEMI", "I dreamt that he came to the lab and asked me to forgive him.")
	speak("ARIEL", "For what?")
	speak("NOEMI", "For Ward No.9, I think. If he had never existed, or never synthesized Bluepill... You and I would've never ended up there.")
	speak("ARIEL", "Those are a lot of 'ifs'. He never had anything to do with us, or that trial. He's a chemist, not a psychiatrist, and definitely not one of those demons in white that experimented on kids.")
	speak("NOEMI", "I want to agree, but...")
	speak("ARIEL", "But what?")
	expr('NOEMI', 'happy')
	speak("NOEMI", "Hm. Haha. I found a bug.")
	speak("ARIEL", "Noemi?")
	speak("NOEMI", "In face recognition. I think I understand most of what's going on now. I'm going to focus on my dream self, and she'll fix the bug. Okay?")
	speak("ARIEL", "Alright.")
	expr('NOEMI', nil)
	speak("ARIEL", "Noemi? Please remember though. You and I aren't lab rats any more. We're free. If there's nothing for us here at Lucir, we can quit and walk free the next day.")
	speak("ARIEL", "...Noemi?")
	expr('NOEMI', 'happy')
	speak("NOEMI", "Hm hm. Free, floating above the rest of the world...")
	speak("ARIEL", "I'll see you once you wake up. Look after yourself, Noemi.")
	exitNVL()
else
	speak("ARIEL", "Noemi's happily fixing her bug, somewhere far away from here.")
end

setSwitch('pt2_04', true)
