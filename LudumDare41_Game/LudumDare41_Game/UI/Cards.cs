using LudumDare41_Game.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.UI {

    class Cards {
        public List<Card> allCards = new List<Card>();
        public static List<HandCard> cardsInHand = new List<HandCard>();
        public static bool anyHeld = false;
        public static HandCard heldCard = null;

        KeyboardState newState, oldState;
        MouseState newMouseState, oldMouseState;
        Card TestTower;


        public Cards() {
            TestTower = new Card("Mage Tower", "A test card for a \ntest tower.", "mageTower", "mageBg", 9);//make a test card
            allCards.Add(TestTower);
            cardsInHand.Add(new HandCard(TestTower));
        }

        public HandCard CurrentlyHeldCard() {
            if(heldCard != null) {
                return heldCard;
            }
            else {
                return null;
            }
        }

        public void Load(ContentManager c) {
            foreach(Card card in allCards) {
                if (!card.isLoaded) {
                    card.Load(c);
                }
            }
        }

        public void Update(GameTime gt, GameWindow w) {
            newState = Keyboard.GetState();
            newMouseState = Mouse.GetState();

            int i = 0;
            foreach(HandCard card in cardsInHand) {
                if(Mouse.GetState().LeftButton.Equals(ButtonState.Pressed)
                    && Mouse.GetState().Position.X > ((w.ClientBounds.Width / 2) - 100) + (220 * i) - (110 * (cardsInHand.Count - 1))
                    && Mouse.GetState().Position.X < ((w.ClientBounds.Width / 2) - 100) + (220 * i) - (110 * (cardsInHand.Count - 1)) + 200
                    && Mouse.GetState().Position.Y > w.ClientBounds.Height - 360
                    && Mouse.GetState().Position.Y < w.ClientBounds.Height - 10
                    && !anyHeld
                    && CardSelector.isActive) {
                    card.isHeld = true;
                    anyHeld = true;
                    heldCard = card;
                }

                if (Mouse.GetState().LeftButton.Equals(ButtonState.Released)) {
                    card.isHeld = false;
                    anyHeld = false;
                    heldCard = null;
                }

                if (card.isHeld) {
                    card.pos = new Vector2(card.pos.X + (newMouseState.Position.X - oldMouseState.Position.X), card.pos.Y + (newMouseState.Position.Y - oldMouseState.Position.Y));
                }
                else {
                    if (CardSelector.isActive) {
                        card.pos = new Vector2(((w.ClientBounds.Width / 2) - 100) + (220 * i) - (110 * (cardsInHand.Count - 1)), w.ClientBounds.Height - 360);
                    }
                    else {
                        card.pos = new Vector2(((w.ClientBounds.Width / 2) - 100) + (220 * i) - (110 * (cardsInHand.Count - 1)), w.ClientBounds.Height - 25);
                    }
                }

                i++;
            }

            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
                cardsInHand.Add(new HandCard(TestTower));

            oldMouseState = newMouseState;
            oldState = newState;
        }

        public void Draw(SpriteBatch sb, GameWindow w) {
            int i = 0;
            foreach (HandCard card in cardsInHand) {
                    card.Draw(sb);
                }
                i++;
            }
        }

    class Card {
        string title, desc, imgNameSrc, bgNameSrc;
        Texture2D cardImg, bg, healthPot;
        SpriteFont titleFont, healthFont;

        public bool isLoaded { get; private set; }
        public int health { get; private set; }

        public Card(string title, string desc, string imgNameSrc, string bgNameSrc, int health) { //dette er de forskjellige Card typene, som kan bli håndtert i Cards classen?
            this.title = title;
            this.desc = desc;
            this.imgNameSrc = imgNameSrc;
            this.bgNameSrc = bgNameSrc;
            this.health = health;
            isLoaded = false;
        }

        public void Load(ContentManager c) {
            cardImg = c.Load<Texture2D>("GUI/Cards/" + title + "/" + imgNameSrc);
            bg = c.Load<Texture2D>("GUI/Cards/" + title + "/" + bgNameSrc);
            titleFont = c.Load<SpriteFont>("GUI/Cards/TitleFont");
            healthPot = c.Load<Texture2D>("Gui/Cards/HealthBottle");
            healthFont = c.Load<SpriteFont>("GUI/Cards/Health");
            isLoaded = true;
        }

        public void Draw(SpriteBatch sb, Vector2 pos) {
            sb.Draw(bg, new Rectangle((int)pos.X, (int)pos.Y, 200, 350), Color.White);
            sb.Draw(cardImg, new Rectangle((int)pos.X + 8, (int)pos.Y + 8, 184, 120), Color.White);
            sb.DrawString(titleFont, title, new Vector2((int)pos.X + 12, (int)pos.Y + 132), Color.Black);
            sb.Draw(healthPot, new Rectangle((int)pos.X + 8, (int)pos.Y + 162, 64, 64), Color.White);
            sb.DrawString(healthFont, health.ToString(), new Vector2((int)pos.X + 60, (int)pos.Y + 185), Color.Black);
        }
    }

    class HandCard {
        public Vector2 pos { get; set; }
        public bool isHeld { get; set; }
        public Card referenceCard { get; private set; }

        public HandCard(Card referenceCard) {
            this.referenceCard = referenceCard;
            pos = new Vector2(-1000, -1000);
        }

        public void Draw(SpriteBatch sb) {
            referenceCard.Draw(sb, pos);
        }
    }
}
