﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;
using TileGame;
using TileEngine;

namespace TileGame.GameScreens
{
    public class NotebookScreen : BaseGameState
    {
        #region Field region

        //Message message;
        int pageIndex = 0;
        int lastPageIndex = 0;
        int leftPageIndex = 0;
        int rightPageIndex = 0;
        int sectionHandler = 0;
        Texture2D backgroundImage;
        Texture2D rightArrow;
        Rectangle rightArrowRect;
        Texture2D leftArrow;
        Rectangle leftArrowRect;
        Texture2D upArrow;
        Rectangle upArrowRect;
        Texture2D downArrow;
        Rectangle downArrowRect;
        
        Dictionary<int, Message> taskDict = new Dictionary<int, Message>();
        Dictionary<int, Message> completedDict = new Dictionary<int, Message>();
        Dictionary<int, Message> hintDict = new Dictionary<int, Message>();

        Dictionary<int, Message> activeDict = new Dictionary<int, Message>();

        List<int> ListOfUnlockedKeys;
        private Label headline;
        Label leftText;
        Label rightText;
        Label leftText2;
        Label rightText2;
        Label leftPagenumberText;
        Label rightPagenumberText;
        Rectangle textRect;
        SoundEffect changePageSound;
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        public NotebookScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }
        #endregion

        #region XNA Method Region
        public override void Initialize()
        {
            ListOfUnlockedKeys = new List<int>();
            //textRect = new Rectangle(0, 0, 400, 600);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\book");
            rightArrow = Content.Load<Texture2D>("GUI/rightarrowUp");
            leftArrow = Content.Load<Texture2D>("GUI/leftarrowUp");
            upArrow = Content.Load<Texture2D>("GUI/arrowUp");
            downArrow = Content.Load<Texture2D>("GUI/arrowDown");
            base.LoadContent();

            float middle = GraphicsDevice.Viewport.Width / 2;
            float middleLeftSide = middle / 2;
            float middleRightSide = middle + middleLeftSide;
            float widthOffset = GraphicsDevice.Viewport.Width / 1248f;
            float heightOffset = GraphicsDevice.Viewport.Height / 768f;
            textRect = new Rectangle(0, 0, (int)(400 * widthOffset), (int)(600 * heightOffset));
            rightArrowRect = new Rectangle((int)middleRightSide + 200 * (int)widthOffset, 900 * (int)heightOffset, 25, 25);
            leftArrowRect = new Rectangle((int)middleLeftSide - 200 * (int)widthOffset, 900 * (int)heightOffset, 25, 25);
            upArrowRect = new Rectangle((int)middleRightSide - 350 * (int)widthOffset, 850 * (int)heightOffset, 25, 25);
            downArrowRect = new Rectangle((int)middleLeftSide + 350 * (int)widthOffset, 850 * (int)heightOffset, 25, 25);

            headline = new Label();
            headline.Position = new Vector2(middleLeftSide - 80 * widthOffset, 150 * heightOffset);
            headline.Text = "";
            headline.Color = Color.DarkBlue;
            headline.SpriteFont.LineSpacing = 1;
            ControlManager.Add(headline);

            leftPagenumberText = new Label();
            leftPagenumberText.Position = new Vector2(middleLeftSide + 40 * widthOffset, GraphicsDevice.Viewport.Height - 170 * widthOffset);
            leftPagenumberText.Text = pageIndex.ToString();
            leftPagenumberText.Color = Color.DarkBlue;
            ControlManager.Add(leftPagenumberText);

            rightPagenumberText = new Label();
            rightPagenumberText.Position = new Vector2(middleRightSide - 70 *widthOffset, GraphicsDevice.Viewport.Height - 160 * widthOffset);
            rightPagenumberText.Text = pageIndex.ToString();
            rightPagenumberText.Color = Color.DarkBlue;
            ControlManager.Add(rightPagenumberText);

            leftText = new Label();
            leftText.Position = new Vector2(middleLeftSide - 140 * widthOffset, 220 * heightOffset); //150, 180
            leftText.Text = "";
            leftText.Color = Color.DarkBlue;
            ControlManager.Add(leftText);

            rightText = new Label();
            rightText.Position = new Vector2(middleRightSide - 200 * widthOffset, 220 * heightOffset);
            rightText.Text = "";
            rightText.Color = Color.DarkBlue;
            ControlManager.Add(rightText);

            leftText2 = new Label();
            leftText2.Position = new Vector2(middleLeftSide - 140 * widthOffset, 450 * heightOffset); //150, 180
            leftText2.Text = "";
            leftText2.Color = Color.DarkBlue;
            ControlManager.Add(leftText2);

            rightText2 = new Label();
            rightText2.Position = new Vector2(middleRightSide - 200 * widthOffset, 450 * heightOffset);
            rightText2.Text = "";
            rightText2.Color = Color.DarkBlue;
            ControlManager.Add(rightText2);

            InsertTextToDictionary(hintDict, 0, "");
            InsertTextToDictionary(hintDict, 1, "Hint: You need an official permit to leave the town.");
            InsertTextToDictionary(hintDict, 2, "Hint: Now you have the axe, you can use it to figth or chop!");
            InsertTextToDictionary(hintDict, 3, "Hint: Some guards like alchohol.");

            InsertTextToDictionary(taskDict, 0, "");
            InsertTextToDictionary(taskDict, 1, "Task: Find a way out of Potato town.");
            InsertTextToDictionary(taskDict, 2, "Task: You have now the Belladonna potato and should move on to next town.");
            InsertTextToDictionary(taskDict, 3, "Task: Asterix told you to find potato The Belladonna. Check out the abandoned fields in the north west.");
            InsertTextToDictionary(taskDict, 4, "Task: Rescue Ariels fish friends.");
            InsertTextToDictionary(taskDict, 5, "Task: Retrive a valid permit.");
            InsertTextToDictionary(taskDict, 6, "Task: Cut down the tree.");

            InsertTextToDictionary(completedDict, 0, "");
            InsertTextToDictionary(completedDict, 1, "Completed: You managed to solve Johns riddle.");
            InsertTextToDictionary(completedDict, 2, "Completed: You managed to beat Johnnys challenge.");
            InsertTextToDictionary(completedDict, 3, "Completed: You completed Jacks errand.");
            InsertTextToDictionary(completedDict, 4, "Completed: You have aquired a valid permit.");
            InsertTextToDictionary(completedDict, 5, "Completed: You talked to Asterix.");
            InsertTextToDictionary(completedDict, 6, "Completed: You collected the Belladona.");
            InsertTextToDictionary(completedDict, 7, "Completed: You cut down the tree.");


            hintDict[0].Unlocked = true;
            taskDict[0].Unlocked = true;
            completedDict[0].Unlocked = true;
            taskDict[1].Unlocked = true;

            changePageSound = Content.Load<SoundEffect>(@"Sounds/Effects/switch_page");
            taskDict[3].Unlocked = true;
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            #region Dict updates

            if (StoryProgress.ProgressLine["treeIsDown"])
            {
                completedDict[7].Unlocked = true;
                taskDict[6].Unlocked = false;
            }
            if (StoryProgress.ProgressLine["asterixTalkedTo"])
            {
                taskDict[1].Unlocked = false;
                taskDict[3].Unlocked = true;
                completedDict[5].Unlocked = true;
            }
            if (StoryProgress.ProgressLine["gateGuardsTalkedTo"])
            {
                hintDict[3].Unlocked = true;
                taskDict[5].Unlocked = true;
                taskDict[2].Unlocked = false;
            }
            if (StoryProgress.ProgressLine["Belladonna"])
            {
                taskDict[1].Unlocked = false;
                taskDict[2].Unlocked = true;
                taskDict[3].Unlocked = false;
                completedDict[6].Unlocked = true;
            }
            if (StoryProgress.ProgressLine["contestAgainstJohnFinished"])
            {
                completedDict[1].Unlocked = true;
            }
            if (StoryProgress.ProgressLine["contestAgainstJohnnyFinished"])
            {
                completedDict[2].Unlocked = true;
            }
            if (StoryProgress.ProgressLine["contestAgainstJackFinished"])
            {
                completedDict[3].Unlocked = true;
            }

            if (StoryProgress.ProgressLine["Permit"])
            {
                completedDict[2].Unlocked = true;
            }
            if (StoryProgress.ProgressLine["Axe"])
            {
                hintDict[2].Unlocked = true;
            }
            #endregion

            SectionCheck();     
            GetKeysThatAreNotLocked();
            SetTexts();

            if (InputHandler.KeyReleased(Keys.N))
                StateManager.PopState();

            PageNumberUpdate();
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            GameRef.spriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            if (pageIndex+4 < ListOfUnlockedKeys.Count)
                GameRef.spriteBatch.Draw(rightArrow, rightArrowRect, Color.White);

            if (pageIndex >= 4)
                GameRef.spriteBatch.Draw(leftArrow, leftArrowRect, Color.White);

            GameRef.spriteBatch.Draw(upArrow, upArrowRect, Color.White);
            GameRef.spriteBatch.Draw(downArrow, downArrowRect, Color.White);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
        #endregion

        #region Game State Method Region
        #endregion

        #region Methods Region

        private void PageNumberUpdate()
        {
            if (InputHandler.KeyReleased(Keys.Left))
            {
                changePageSound.Play();
                pageIndex -= 2;
                pageIndex -= 4;
                if (pageIndex < 0)
                    pageIndex = 0;
                //else
                //  changePageSound.Play();
            }
            if (InputHandler.KeyReleased(Keys.Right))
            {
                changePageSound.Play();
                lastPageIndex = pageIndex;
                pageIndex += 4;
                if (pageIndex > ListOfUnlockedKeys.Count - 2)
                    pageIndex = lastPageIndex;
                //else
                //    changePageSound.Play();
            }
        }

        private void InsertTextToDictionary(Dictionary<int, Message> messageDict,int key, string text)
        {
            StringBuilder stringBuild = new StringBuilder();
            string formatedText;
            //text = "Nu maste du hitta nyckeln, den ligger dar man minst anar";
            WordWrapper.WrapWord(new StringBuilder(text), stringBuild, leftText.SpriteFont, textRect);
            formatedText = stringBuild.ToString();
            if (!messageDict.ContainsKey(key))
                messageDict.Add(key, new Message(formatedText));
            else
                messageDict[key].Text = formatedText;
        }

        private void GetKeysThatAreNotLocked()
        {
            ListOfUnlockedKeys.Clear();
            foreach(KeyValuePair<int, Message> pair in activeDict)
            {
                if (pair.Value.Unlocked)
                    ListOfUnlockedKeys.Add(pair.Key);
            }
            ListOfUnlockedKeys.Sort();
        }

        private Color GetTextColor(string text)
        {
            if (text == "Hint")
                return Color.ForestGreen;
            else if (text == "Completed")
                return Color.IndianRed;
            else if (text == "Task")
                return Color.DarkBlue;
            else
            {
                return Color.DarkBlue;
            }
        }

        private void SectionCheck()
        {
            if (InputHandler.KeyReleased(Keys.Up))
            {
                pageIndex = 0;
                sectionHandler--;
                if (sectionHandler < 0)
                    sectionHandler = 2;
            }
            if (InputHandler.KeyReleased(Keys.Down))
            {
                pageIndex = 0;
                sectionHandler++;
                if (sectionHandler > 2)
                    sectionHandler = 0;
            }
            if (sectionHandler == 0)
            {
                activeDict.Clear();
                headline.Text = "Current tasks";
                headline.color = Color.DarkBlue;
                foreach (var key in taskDict)
                {
                    if (taskDict[key.Key].Unlocked)
                        activeDict.Add(key.Key, key.Value);
                }
            }
            else if (sectionHandler == 1)
            {
                activeDict.Clear();
                headline.Text = "Hints";
                headline.color = Color.ForestGreen;
                foreach (var key in hintDict)
                {
                    if (hintDict[key.Key].Unlocked)
                        activeDict.Add(key.Key, key.Value);
                }
            }
            else if (sectionHandler == 2)
            {
                activeDict.Clear();
                headline.Text = "Completed tasks";
                headline.color = Color.IndianRed;
                foreach (var key in completedDict)
                {
                    if (completedDict[key.Key].Unlocked)
                        activeDict.Add(key.Key, key.Value);
                }
            }
        }

        private void SetTexts()
        {
           /* leftText.Text = "";
            leftText2.Text = "";
            rightText.Text = "";
            rightText2.Text = "";*/

            if (pageIndex < (ListOfUnlockedKeys.Count))
            {
                if (pageIndex + 1 < ListOfUnlockedKeys.Count)
                    leftPageIndex = ListOfUnlockedKeys[pageIndex + 1];
                else
                    leftPageIndex = 0;
                if (activeDict.ContainsKey(leftPageIndex))
                {
                    leftText.Text = activeDict[leftPageIndex].Text;
                    leftText.Color = GetTextColor(leftText.Text.Split(':')[0]);
                    if (activeDict.ContainsKey(leftPageIndex + 1) && leftPageIndex != 0)
                    {
                        leftText2.Text = activeDict[leftPageIndex + 1].Text;
                        leftText2.Color = GetTextColor(leftText2.Text.Split(':')[0]);
                    }
                    if (pageIndex == 0)
                        leftPagenumberText.Text = "1";
                    else
                    {
                        leftPagenumberText.Text = (pageIndex / 2 + 1).ToString();
                    }
                }

                if (pageIndex + 3 < ListOfUnlockedKeys.Count)
                    rightPageIndex = ListOfUnlockedKeys[pageIndex + 3];
                else
                    rightPageIndex = 0;
                if (activeDict.ContainsKey(rightPageIndex))
                {
                    rightText.Text = activeDict[rightPageIndex].Text;
                    rightText.Color = GetTextColor(rightText.Text.Split(':')[0]);
                    if (activeDict.ContainsKey(rightPageIndex + 1) && rightPageIndex != 0)
                    {
                        rightText2.Text = activeDict[rightPageIndex + 1].Text;
                        rightText2.Color = GetTextColor(rightText2.Text.Split(':')[0]);
                    }
                    if (pageIndex == 0)
                        rightPagenumberText.Text = "2";
                    else
                    {
                        rightPagenumberText.Text = (pageIndex / 2 + 2).ToString();
                    }
                }
            }
        }

        public class WordWrapper 
	    { 
	        public static char[] NewLine = {'\r','\n'}; 
	        public static void WrapWord(StringBuilder original, StringBuilder target, SpriteFont font, Rectangle bounds) 
	        { 
	            int lastWhiteSpace = 0; 
	            Vector2 currentTargetSize; 
	            for (int i = 0; i < original.Length; i++) 
	            {                 
	                char character = original[i]; 
	                if (char.IsWhiteSpace(character)) 
	                { 
	                    lastWhiteSpace = target.Length; 
	                } 
	                target.Append(character); 
	                currentTargetSize = font.MeasureString(target); 
	                if (currentTargetSize.X > bounds.Width) 
	                {                     
	                    target.Insert(lastWhiteSpace, NewLine); 
	                    target.Remove(lastWhiteSpace + NewLine.Length, 1); 
	                } 
	            } 
	        } 
	    }
        #endregion
    }
}
