//
// Copyright 2011-2012 Xamarin Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;

using Android.App;
using Android.Content;
using Android.Views;

using NUnitLite;
using NUnit.Framework.Internal;

namespace Android.NUnitLite.UI {
	
	class TestSuiteElement : TestElement {

		public TestSuiteElement (TestSuite suite) : base (suite)
		{
			if (Suite.TestCaseCount > 0)
				Indicator = ">"; // hint there's more
		}
		
		public TestSuite Suite {
			get { return Test as TestSuite; }
		}
		
		protected override string GetCaption ()
		{
			int count = Suite.TestCaseCount;
			string caption = String.Format ("<b>{0}</b><br>", Suite.Name);
			if (count == 0) {
				caption += "<font color='#ff7f00'>no test was found inside this suite</font>";
			} else if (Result == null) {
				caption += String.Format ("<font color='green'><b>{0}</b> test case{1}, <i>{2}</i></font>", 
					count, count == 1 ? String.Empty : "s", Suite.RunState);
			} else {

				int failure = Result.FailCount;
				int success = Result.PassCount;
				int inconclusive = Result.InconclusiveCount;
				int skipped =  Result.SkipCount;

				
				if (Result.ResultState.Status == NUnit.Framework.Api.TestStatus.Passed) {
					caption += String.Format ("<font color='lime'><b>Success!</b> {0} test{1}</font>",
						success, success == 1 ? String.Empty : "s");
				} else {
					caption += String.Format ("<font color='lime'>{0} passed,</font> <font color='red'>{1} failed,</font> <font color='yellow'>{2} ignored,</font>"
					                          + " <font color='#FF00FF'>{3} inconclusive</font>", success, failure, skipped, inconclusive);				
				}
			}
			return caption;
		}

		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			View view = base.GetView (context, convertView, parent);
			// if there are test cases inside this suite then create an activity to show them
			if (Suite.TestCaseCount > 0) {		
				view.Click += delegate {
					Intent intent = new Intent(context, typeof (TestSuiteActivity));
					intent.PutExtra ("TestSuite", Name);
					intent.AddFlags (ActivityFlags.NewTask);			
					context.StartActivity (intent);
				};
			}
			return view;
		}
	}
}