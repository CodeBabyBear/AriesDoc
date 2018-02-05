using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Pandv.AriesDoc.Generator.Comments;
using Pandv.AriesDoc.Generator.RAML;

namespace Pandv.AriesDoc.Generator
{
    public class XmlComments : IComments
    {
        private const string MemberXPath = "/doc/members/member[@name='{0}']";
        private const string SummaryXPath = "summary";
        //private const string RemarksXPath = "remarks";
        private const string ParamXPath = "param[@name='{0}']";
        private const string ResponsesXPath = "response";

        private XPathDocument doc;
        private XPathNavigator navigator;

        public XmlComments(string xmlCommentsFile)
        {
            if (File.Exists(xmlCommentsFile))
            {
                doc = new XPathDocument(xmlCommentsFile);
                navigator = doc.CreateNavigator();
            }
        }

        private bool Cannavigator() => navigator != null;

        private string GetCommentIdForMethod(MethodInfo methodInfo)
        {
            var builder = new StringBuilder("M:");
            AppendFullTypeName(methodInfo.DeclaringType, builder);
            builder.Append(".");
            AppendMethodName(methodInfo, builder);

            return builder.ToString();
        }

        private void AppendFullTypeName(Type type, StringBuilder builder, bool expandGenericArgs = false)
        {
            if (type.Namespace != null)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            AppendTypeName(type, builder, expandGenericArgs);
        }

        private void AppendTypeName(Type type, StringBuilder builder, bool expandGenericArgs)
        {
            if (type.IsNested)
            {
                AppendTypeName(type.DeclaringType, builder, false);
                builder.Append(".");
            }

            builder.Append(type.Name);

            if (expandGenericArgs)
                ExpandGenericArgsIfAny(type, builder);
        }

        public void ExpandGenericArgsIfAny(Type type, StringBuilder builder)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                var genericArgsBuilder = new StringBuilder("{");

                var genericArgs = type.GetGenericArguments();
                foreach (var argType in genericArgs)
                {
                    AppendFullTypeName(argType, genericArgsBuilder, true);
                    genericArgsBuilder.Append(",");
                }
                genericArgsBuilder.Replace(",", "}", genericArgsBuilder.Length - 1, 1);

                builder.Replace(string.Format("`{0}", genericArgs.Length), genericArgsBuilder.ToString());
            }
            else if (type.IsArray)
                ExpandGenericArgsIfAny(type.GetElementType(), builder);
        }

        private void AppendMethodName(MethodInfo methodInfo, StringBuilder builder)
        {
            builder.Append(methodInfo.Name);

            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0) return;

            builder.Append("(");
            foreach (var param in parameters)
            {
                AppendFullTypeName(param.ParameterType, builder, true);
                builder.Append(",");
            }
            builder.Replace(",", ")", builder.Length - 1, 1);
        }

        public void SetCommentToMethod(ApiDescription api, Method method)
        {
            if (Cannavigator() && api.ActionDescriptor is ControllerActionDescriptor controller)
            {
                var id = GetCommentIdForMethod(controller.MethodInfo);
                var methodNode = navigator.SelectSingleNode(string.Format(MemberXPath, id));
                if (methodNode == null) return;
                var summaryNode = methodNode.SelectSingleNode(SummaryXPath);
                if (summaryNode != null)
                    method.Description.Value = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
                ApplyResponsesXmlToResponses(method.Responses, methodNode.Select(ResponsesXPath));
                ApplyParamsXmlToActionParameters(method.QueryParameters, methodNode);
            }
        }

        private void ApplyParamsXmlToActionParameters(ArrayElement queryParameters, XPathNavigator methodNode)
        {
            if (!queryParameters.HasElements) return;
            foreach (var item in queryParameters.Elements)
            {
                var paramNode = methodNode.SelectSingleNode(string.Format(ParamXPath, item.Key));
                if(paramNode != null && item is Parameter p)
                    p.Description.Value = XmlCommentsTextHelper.Humanize(paramNode.InnerXml);
            }

        }

        private void ApplyResponsesXmlToResponses(ArrayElement responses, XPathNodeIterator responseNodes)
        {
            while (responseNodes.MoveNext())
            {
                var code = responseNodes.Current.GetAttribute("code", "");
                var response = responses.TryGetElement<Response>(code);
                if (response == null) continue;
                response.Description.Value = XmlCommentsTextHelper.Humanize(responseNodes.Current.InnerXml);
            }
        }

        public void SetCommentToUriParameters(ArrayElement uriParameters, ApiDescription api)
        {
            if (Cannavigator() && api.ActionDescriptor is ControllerActionDescriptor controller)
            {
                var id = GetCommentIdForMethod(controller.MethodInfo);
                var methodNode = navigator.SelectSingleNode(string.Format(MemberXPath, id));
                if (methodNode == null) return;
                ApplyParamsXmlToActionParameters(uriParameters, methodNode);
            }
        }
    }
}