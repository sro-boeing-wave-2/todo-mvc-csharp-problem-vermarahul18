using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using google_keep.Controllers;
using google_keep.Models;

namespace TEST
{
    public class Integrate_testing
    {
        private readonly NotesController _controller;
        private NoteContext notecontext;
        public Integrate_testing()
        {
            var optionBuilder = new DbContextOptionsBuilder<NoteContext>();
            optionBuilder.UseInMemoryDatabase("any string");
            notecontext = new NoteContext(optionBuilder.Options);
            notecontext.Note.AddRange(notebyid);
            notecontext.SaveChanges();


            _controller = new NotesController(notecontext);


        }
        Note TestNotePost = new Note
        {
            Title = "Title-2-Deletable",
            text = "Message-2-Deletable",
            checklist = new List<CheckList>()
                       {
                           new CheckList(){ Check = "checklist-2-1", isChecked = true},
                           new CheckList(){ Check = "checklist-2-2", isChecked = false}
                       },
            labels = new List<Labels>()
                       {
                           new Labels(){label = "Label-2-1-Deletable"},
                           new Labels(){ label = "Label-2-2-Deletable"}
                       },
            Pinned = false
        };
        Note TestNotePut = new Note
        {
            Id = 1,
            Title = "Title-1-Deletable",
            text = "Message-1-Deletable",
            checklist = new List<CheckList>()
                       {
                           new CheckList(){ Check = "checklist-1", isChecked = true},
                           new CheckList(){ Check = "checklist-2", isChecked = false}
                       },
            labels = new List<Labels>()
                       {
                           new Labels(){label = "Label-1-Deletable"},
                           new Labels(){ label = "Label-2-Deletable"}
                       },
            Pinned = false
        };

        Note notebyid = new Note()
        {
            Title = "this is test title",
            text = "some text",
            labels = new List<Labels>
                {
                    new Labels{ label="My First Tag" },
                    new Labels{ label = "My second Tag" },
                    new Labels{ label = "My third Tag" },
                },
            checklist = new List<CheckList>
                {
                    new CheckList{Check="first item"},
                    new CheckList{Check="second item"},
                    new CheckList{Check="third item"},
                },
            Pinned = true
        };

        //public void createtestnotes(NoteContext notesContext)
        //{
        //    var note = new Note()
        //    {
        //        Title = "this is test title",
        //        text = "some text",
        //        labels = new List<Labels>
        //        {
        //            new Labels{ label="My First Tag" },
        //            new Labels{ label = "My second Tag" },
        //            new Labels{ label = "My third Tag" },
        //        },
        //        checklist = new List<CheckList>
        //        {
        //            new CheckList{Check="first item"},
        //            new CheckList{Check="second item"},
        //            new CheckList{Check="third item"},
        //        },
        //        Pinned = true
        //    };

        //    notesContext.Note.AddRange(note);
        //    notesContext.SaveChanges();
        //}

  

        [Fact]
        public async void TestGetNotes()
        {
            //createtestnotes(notecontext);
            var result = await _controller.GetNoteByPrimitive(0, null, null, true);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Assert.Equal(1, notes.Count);
        }

        [Fact]
        public async void TestGetByID()
        {

            var result =  await _controller.GetNoteByPrimitive(6, null, null, false);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Console.WriteLine(notes);
            Assert.Equal(notes[0], notebyid);
        }

        [Fact]
        public async void TestPostNotes()
        {
            var response = await _controller.PostNote(TestNotePost);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            Assert.Equal(note.Title, TestNotePost.Title);
        }


        [Fact]
        public async void Put()
        {
            var response = await _controller.PutNote(1, TestNotePut);
            var responseOkObject = response as OkObjectResult;
            Note note = responseOkObject.Value as Note;
            //Console.WriteLine(GetNoteByPrimitive.Id);
            Assert.Equal(note.Title, TestNotePut.Title);
        }
    

        [Fact]
        public async void Delete()
        {
            var result = await _controller.DeleteNote(0, null, null, true);
            Assert.True(result is NoContentResult);
        }


    }
}