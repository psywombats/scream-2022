enterNVL()
enter('BRAULIO', 'b')

if not getSwitch('pt1_05d') then
	enter('SUMI', 'd')
	speak('BRAULIO', "Oh hey Ariel. And welcome back, Sumi.")
	speak('BRAULIO', "Sorry for the wait! We just weren't expecting you this early. This whole thing's been a whirlwind.")
	if getSwitch('pt1_05a') and getSwitch('pt1_05b') and getSwitch('pt1_05c') then
		expr('BRAULIO', 'unsure')
		speak('BRAULIO', "Hopefully I should wrap things up once Ariel's done showing you around.")
		expr('BRAULIO', nil)
		speak('SUMI', "My apologies for the inconvenience.")
	end
	expr('BRAULIO', nil)
	exit('SUMI')
	enter('SUMI', 'e', 'surprised')
	speak('SUMI', "I love that statuette on your desk, by the way. Is that a goat?")
	expr('SUMI', nil)
	speak('BRAULIO', "A mountain goat, to be specific! I love them.")
	speak('BRAULIO', "I'm pretty big into climbing. Or I was, when I still had time to do anything outside Lucir.")
	speak('BRAULIO', "Sophomore year, I took a trip up into the Cascades. I was out on a cliff face probably 2000 feet above the treeline and saw two of those goats up near the summit, on a 70 degree incline.")
	speak('ARIEL', "They made it to the top?")
	speak('BRAULIO', "One did. The other, well, fell and died.")
	speak('SUMI', "Ahaha. A story with a moral.")
	expr('BRAULIO', 'unsure')
	speak('BRAULIO', "Sorry, that was probably really inappropriate.")
	expr('BRAULIO', 'determined')
	speak('BRAULIO', "But the point is, they're fearless animals that don't stop until they reach the top. I admire them.")
	expr('BRAULIO', nil)
	speak('BRAULIO', "Maybe that's why having the Lucir lab in such a high place appealed to me.")
	exit('SUMI')
	enter('SUMI', 'd')
	speak('SUMI', "The building certainly is unique. Is this tower owned by Aquila University?")
	speak('BRAULIO', "Yep! We're renting floors 36 and 37 from Aquila.")
	speak('BRAULIO', "Everyone working here met there. Noemi and Ariel are graduates, Chris used to teach there, and, uh, well, I dropped out. Lucir was more important than my degree.")
	speak('SUMI', "That's a spirit I love to see in founders.")
	if getSwitch('pt1_05a') and getSwitch('pt1_05b') and getSwitch('pt1_05c') then
		speak('BRAULIO', "I'm actually all ready if you want to talk financials now.")
		speak('ARIEL', "We're all done with the tour, anyway.")
		expr('SUMI', 'intense')
		speak('SUMI', "Excellent. Let's get to it.")
		exit('SUMI')
		exit('BRAULIO')
		wait(1.0)
		speak('ARIEL', "Good luck, Braulio.")
		wait(.5)
		play('pt1_06')
	else
		speak('ARIEL', "Sorry to interrupt you, Braulio. We'll get on with the tour and then be back when you're ready.")
		speak('BRAULIO', "No problem! It was nice talking to you, Sumi!")
	end
else
	enterNVL()
	enter('BRAULIO', 'c')
	speak('ARIEL', "Sorry to interrupt you, Braulio. We'll get on with the tour and then be back when you're ready.")
	speak('BRAULIO', "No problem! It was nice talking to you!")
	exitNVL()
end

setSwitch('pt1_05d', true)
