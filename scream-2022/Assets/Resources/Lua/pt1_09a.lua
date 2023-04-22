enterNVL()
enter('BRAULIO', 'b')

if not getSwitch('pt1_09a') then
	speak('ARIEL', "How's it going? You look tense.")
	speak('BRAULIO', "I'm alright. It's probably just the third coffee of the day kicking in, haha.")
	expr('BRAULIO', 'determined')
	speak('BRAULIO', "I was just reading up on some of the other companies the Chey group has invested in.")
	speak('ARIEL', "...Shouldn't you have done that beforehand?")
	speak('BRAULIO', "Maybe. But I couldn't find any. As far as I know, they haven't supplied venture capital to any other startups.")
	speak('BRAULIO', "All of these companies are outright owned by the central holdings group, and they're all in heavy industry. No tech or bio tech to be found. Is that bad?")
	expr('BRAULIO', nil)
	speak('BRAULIO', "Sorry, I'm just really nervous about underdelivering.")
	speak('ARIEL', "It's fine. Sumi saw the mirroring demo, right?")
	speak('BRAULIO', "She seems really interested in crafting specific dreams, or something like that. I may have mentioned the editor project I was working on.")
	speak('ARIEL', "Dream editing? Does that actually work?")
	expr('BRAULIO', 'unsure')
	speak('BRAULIO', "It really doesn't. I sort of gave up on it once you and Noemi got mirroring working.")
	expr('BRAULIO', nil)
	speak('BRAULIO', "Why edit dreams when you can craft new ones in realtime, right?")
	speak('ARIEL', "Does Sumi think we're going to try and develop the editor?")
	speak('BRAULIO', "I don't know. I don't really feel like asking. Maybe I'll start working on editing again, once we've got the new Gazer prototype built.")
	speak('ARIEL', "Sounds good.")
else
	speak('BRAULIO', "I think Chris was looking for you in the chemistry lab?")
end

exitNVL()
setSwitch('pt1_09a', true)
