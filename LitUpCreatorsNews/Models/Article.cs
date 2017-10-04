using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CreatorsNews.Models
{
    public class Article
    {
        public ImageSource MainImage { get; set; }
        public string[] Paragraphs { get; set; }
        public string Title { get; set; }

        public int ColSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;

        public static Article[] GenerateArticles()
        {
            return new[]
            {
                new Article
                {
                    Title = "Beyond Good & Evil 2 Promises a Planet-Sized RPG",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/Beyond.Png")),
                    RowSpan = 2,
                    ColSpan = 2,
                    Paragraphs = new[]
                    {
                        "I can’t recall how many times during my meeting with the development team of Beyond Good & Evil 2" +
                        " I thought to myself, “This is impossible.” While the trailer shown at Ubisoft’s E3 conference was" +
                        " more of a development announcement, our post-show meeting was where the concept and true potential" +
                        " of BG&E2 really came into focus. The goal is a seamless multi-planet world populated by " +
                        "dynamically-generated characters, with unique stories being told and optional social interaction " +
                        "throughout. I may have initially scoffed at these ambitious ideas, but by the end of our meeting " +
                        "I was enthralled, not to mention giggling like a child out of sheer amazement at the fact that this " +
                        "could really work.",
                        "What we actually saw was a demonstration of the team’s new engine, one they've been working on for" +
                        " the better part of three years, and the potential that it offers for the world they want to build." +
                        " 'We wanted to emphasize with this game is the sense of scale,' explained Ubisoft Montpellier's " +
                        "senior producer Guillaume Brunier, “So we developed the technology to let us go from a very small, " +
                        "tiny place in a big city [all the way] to space exploration.” Referring to some nearby concept art, " +
                        "he described the idea that players could go from within an intimate location, like a noodle bar or " +
                        "back alley, and walk outside through the fully-designed city, then hop into a ship and fly to the " +
                        "other side of the planet, or evey beyond that to another world entirely - all without load times or " +
                        "transitions."
                    }
                },
                new Article
                {
                    Title = "Nintendo on Switch VC: \"We Recognize There's an Appetite\"",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/NintendoSwitch.png")),
                    Paragraphs = new[]
                    {
                        "While Nintendo has yet to announce plans to bring Virtual Console to Switch, the company is well " +
                        "aware of the demand for legacy content on the console.",
                        "\"We know that our fans, our players, want access to all of our digital content, we know that,\" " +
                        "Nintendo of America president and chief operating officer Reggie Fils-Aime told IGN when asked if " +
                        "the company is interested in bringing Virtual Console to Switch.",
                        "According to the Nintendo exec, the company is currently working on finding the best way to deliver " +
                        "its classic content to Switch owners. \"What we're working through is, 'okay, what's going to be " +
                        "the best way to make that happen, to make that available?'\" Fils-Aime said. \"Certainly, we " +
                        "recognize there's an appetite for all of our great legacy content.\"",
                        "Nintendo previously confirmed that its upcoming subscription service Classic Game Selection isn't " +
                        "designed to be a replacement for Virtual Console.",
                        "Fils-Aime also discussed My Nintendo and what the future holds for the company's rewards program. " +
                        "\"From the Nintendo of America standpoint, we have it as a priority to make My Nintendo much more " +
                        "meaningful moving forward,\" he said. When asked if physical rewards might ever come to the " +
                        "program, Fils-Aime said \"it could\" before discussing a few of the challenges that come with " +
                        "doing something like that in the Americas."
                    }
                },
                new Article
                {
                    Title = "Nintendo Working to Scale Up Switch Production",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/NintendoScale.png")),
                    Paragraphs = new[]
                    {
                        "Nintendo knows it's not meeting Switch demand currently, but the company is working to improve " +
                        "the situation.",
                        "\"What we are doing, as quickly as we can, is scaling up the production to make more available into " +
                        "the marketplace, to get to the point where every consumer who wants a Nintendo Switch can find a " +
                        "Nintendo Switch,\" Reggie Fils-Aime, Nintendo of America's president and chief operating officer " +
                        "told IGN in an interview at E3 2017.",
                        "\"That’s what we’re trying to get to. I’m not going to tell you when we’ll get there, but our goal " +
                        "is to improve our supply chain so that more product is available to more consumers. And that is a " +
                        "key focus.\"",
                        "Nintendo sold over 900,000 Switch systems in March in the United States, followed by over 280,000 in " +
                        "April. Worldwide, Nintendo sold more than 2.74 million Switch consoles in its launch month, " +
                        "surpassing the company's original goal of 2 million."
                    }
                },
                new Article
                {
                    Title = "Pikmin 4 'Progressing,' Says Shigeru Miyamoto",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/Pikmin.png")),
                    Paragraphs = new[]
                    {
                        "Development on Pikmin 4 is still well underway, according to Shigeru Miyamoto.",
                        "In an interview with Eurogamer Miyamoto said that, even though he can't share any new details " +
                        "about the game, Pikmin 4 is \"progressing.\" In a separate statement, Nintendo followed up on " +
                        "Miyamoto's comments, confirming \"that Pikmin 4 is in development but that is all we can " +
                        "confirm at present.\"",
                        "The fourth installment in the series was first announced back in 2015, when Miyamoto revealed " +
                        "the game was \"actually very close to completion\" and that the \"Pikmin teams are always " +
                        "working on the next one.\" A year later, Miyamoto revealed that Pikmin 4 fell down \"a list " +
                        "of priorities\" at Nintendo, which was why there have been little to no updates on the game " +
                        "since its announcement.",
                        "With Pikmin 4 seemingly far off, fans have the 3DS spin-off side-scrolling game Hey! Pikmin to " +
                        "look forward, which releases on July 28. Read IGN's preview of the title here. The last entry " +
                        "in the much-loved franchise – Pikmin 3 – released on August 2013, with IGN's review finding " +
                        "it to be nostalgic, evocative, and clever."
                    }
                },
                new Article
                {
                    Title = "Life is Strange: Before The Storm is For the Fans",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/LifeIsStrange.png")),
                    Paragraphs = new[]
                    {
                        "Life is Strange: Before the Storm does not feature any supernatural powers. There will be no " +
                        "rewinding in Chloe Price’s 16-year-old life, no alternate realities or time-bending. This is not " +
                        "Life is Strange 2, which DontNod is quietly working on in Paris.",
                        "Instead, Before the Storm is a prequel developed by Colorado-based Deck Nine Games, and it’s focused " +
                        "on Chloe Price and her relationship with Rachel Amber. And relationships, says Deck Nine, are what " +
                        "fans loved from the original.",
                        "It’s certainly what I loved from the original, and what I’ve seen from Before the Storm, it’s going " +
                        "to offer all the wonderful angst that comes from close teenage friendships, or indeed, romantic " +
                        "relationships. Chloe and Rachel’s relationship was ambiguous in the original Life is Strange, " +
                        "which opened up a lot of options for exploring what it might have looked like in the prequel.",
                        "Playing as Chloe looks very similar to playing as Max, minus the time-travelling (she even has her " +
                        "own version of photography in graffiting). What’s markedly different is her attitude, and the more " +
                        "mischievous options that are available to her in Arcadia Bay.",
                        "The beginning of the hands-off E3 demo, for example, saw Chloe sneaking into an illegal rock concert " +
                        "in an old mill at the edge of town. She spots a t-shirt vendor leaning against a car trying to sling " +
                        "band tees for 20 bucks, which she does not have. Walking around to the driver’s side of the car, " +
                        "Chloe is given the option to release the handbrake. She does, the vendor goes flying, and Chloe " +
                        "steals both the t-shirt and the $200 bucks resting under it."
                    }
                },
                new Article
                {
                    Title = "Everything You Need to Know About Super Mario Odyssey",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/SuperMario.png")),
                    Paragraphs = new[]
                    {
                        "Nintendo first showed footage of Super Mario Odyssey back in October 2016. Assuming all goes according " +
                        "to plan, you’ll be able to play it on October 27, 2017.",
                        "In Super Mario Odyssey, Mario leaves the Mushroom Kingdom and visits all kinds of new places. Most " +
                        "strikingly, one of them looks very much like our world, complete with skyscrapers and humans who are " +
                        "much taller than the cartoonish plumber. Read on for everything we know so far about Super Mario Odyssey.",
                        "During Nintendo's E3 video presentation, we finally got  a release date for Super Mario Odyssey: October " +
                        "27, 2017. We also got a new trailer that showed a ton of things we'd never seen before, including Mario " +
                        "throwing his hat on enemies and objects to inhabit them."
                    }
                },
                new Article
                {
                    Title = "Crackdown 3: Microsoft clarifies Terry Crews' Role",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/Crackdown.png")),
                    Paragraphs = new[]
                    {
                        "Crackdown 3's new trailer definitely included Terry Crews, but it wasn't entirely clear if he would be " +
                        "in the game. Thankfully, he officially is, and he will be a playable character in some form.",
                        "Speaking to IGN, design director Garett Wilson said, \"He's a playable character. I think I'm OK to tell " +
                        "you that. I probably shouldn't say any more. He definitely is a playable character, and he's a character " +
                        "in the narrative, so he's in the opening cutscene and things like that.\"",
                        "It's unclear whether Crews' character, Commander Jaxon will be playable in the campaign (he did not appear " +
                        "to be pickable in the E3 campaign hands-on), or would be playable in the game's multiplayer modes. Wilson " +
                        "did confirm that both the actor's voice and likeness would be used.",
                        "Wilson also explained the unusual way Crews got involved in the project:",
                        "\"That was crazy. We got this call from marketing, and they were like, 'Terry Crews wants to be in the game.' " +
                        "We were like, 'that'd work. That'd absolutely work.' He's a real-life Agent.\""
                    }
                },
                new Article
                {
                    Title = "Unreal Engine Powering Yoshi Switch",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/Yoshi.png")),
                    Paragraphs = new[]
                    {
                        "Nintendo's upcoming Yoshi game for Switch is being developed with Unreal Engine 4.",
                        "Epic Games senior marketing manager Dana Cowley shared the news in a post on Twitter.",
                        "The adorable platformer is one of many upcoming Switch games being developed with Unreal Engine 4. In fact,  " +
                        "Epic Games Japan rep Takayuki Kawasaki told Automaton (translation via Gematsu) that \"in Japan, there are " +
                        "about 20 titles being developed that use Unreal Engine.\"",
                        "Yoshi for Nintendo Switch was announced earlier this week at E3. The 2D side-scroller features a " +
                        "cardboard-themed setting and will be released sometime in 2018."
                    }
                },
                new Article
                {
                    Title = "Metroid: Samus Returns is not Metroid Dread",
                    MainImage = new BitmapImage(new Uri("ms-appx:///Assets/ArticleImages/MetroidSamus.png")),
                    Paragraphs = new[]
                    {
                        "Metroid: Samus Returns might be a re-imagining of Metroid II on Game Boy, but it's definitely not linked to " +
                        "the rumored Metroid Dread on Nintendo DS.",
                        "Yoshio Sakamoto, who's the co-creator of Metroid and a longtime series producer, shot down the connection in " +
                        "an interview with IGN. \"I think it’s better to say that this is a remake, remastering of Metroid II, a " +
                        "powered-up version of that, and not something to do with the other project,\" he said.",
                        "For those unfamiliar, Metroid Dread was on an official internal Nintendo software list back in 2005 that " +
                        "IGN saw. On top of that, Metroid Prime 3 hinted at Metroid Dread, saying it was \"nearing the final stages of completion.\"",
                        "Metroid: Samus Returns was announced during Nintendo Treehouse Live at E3. The 3DS game is getting a special edition with a " +
                        "Samus CD as well as two amiibo figures."
                    }
                }
            };
        }
    }
}