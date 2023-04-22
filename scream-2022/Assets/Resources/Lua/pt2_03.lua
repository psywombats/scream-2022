enterNVL()
enter('BRAULIO', 'c')

if not getSwitch('pt2_03') then
	speak('ARIEL', "Braulio?")
	expr('BRAULIO', 'unsure')
	speak('BRAULIO', "Oh! Hi! Ariel, right?")
	speak('ARIEL', "You sound like Noemi, trying to ID me my voice instead of my face.")
	expr('BRAULIO', nil)
	speak('BRAULIO', "Haha. Nah, my hearing's probably not as good as hers.")
	speak('ARIEL', "Her hearing is atrocious. Everything's filtered through her dream consciousness.")
	expr('BRAULIO', 'determined')
	speak('BRAULIO'," Do.. do you think that's weird?")
	speak('ARIEL', "Noemi? Everything about her is weird, but I love her all the same.")
	expr('BRAULIO', nil)
	speak('BRAULIO', "Yeah, she's great, and I know you two have been through a lot together.")
	expr('BRAULIO', 'unsure')
	speak('BRAULIO', "Ughh, I'm sorry about this.")
	speak('ARIEL', "About what?")
	expr('BRAULIO', nil)
	speak('BRAULIO', "I'm sorry, I just don't know how to treat you. You're doing you're best for Lucir, and so should I.")
	expr('BRAULIO', 'determined')
	speak('BRAULIO', "Things might seem strange for the next week or so, but if you can bear with it until Sumi is out of here... Man, that would help a lot.")
	expr('BRAULIO', nil)
	speak('BRAULIO', "And this is mirroring demo thing, uh, well, is kind of in the contract. Sorry.")
else
	speak('BRAULIO', "Things might seem strange for the next week or so, but if you can bear with it until Sumi is out of here... Man, that would help a lot.")
end

exitNVL()
setSwitch('pt2_03', true)

