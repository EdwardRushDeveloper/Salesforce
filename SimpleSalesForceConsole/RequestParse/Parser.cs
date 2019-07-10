using System;
using SimpleSalesforce;

namespace SimpleSalesForceConsole.RequestParse
{
    public class Parser
    {
        public Parser()
        {
        }


        public static void ValidResponseParser()
        {
            //string regularExpression = @"(?:(?=(?:finsmaaa:\/\/sflogin)).*?[#?]|(?!.*?[#?])^|&)([^= \n]+?)=([^& \n]+?)(?=$|&)";
            string regularExpression = @"(?<SchemaName>finsmaaa:\/\/sflogin)|(?:(?:(?:(?:#|&)\b(?<PropertyName>access_token|refresh_token|instance_url|id|issued_at|signature|id_token|state|scope|token_type)\b))=(?<PropertyValue>[^& \n]+?)(?=$|&))";
            //string source = @"finsmaaa://sflogin#access_token=00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf&refresh_token=5Aep861E3ECfhV22nYuzbKoDWYBlo3oTokN1ATS_.dGHyLC5GGMTkXjKFtThUIi3sVuy724DNCb3JDbiAymUtqb&instance_url=https%3A%2F%2Fpksf-dev-ed.my.salesforce.com&id=https%3A%2F%2Flogin.salesforce.com%2Fid%2F00Dj0000å0028gCuEAI%2F0053Z00000KPlpVQAT&issued_at=1561384497680&signature=ed1%2F6R2Z%2BtBHwLdlXuUcb3SD0yTKkDdV0J6NL%2FnlpIM%3D&sfdc_community_url=https%3A%2F%2Fmypksåcommunity-developer-edition.na102.force.com%2FNTOCustomers&sfdc_community_id=0DB3Z0000008QM8WAM&id_token=eyJraWQiOiIyMjAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdF9oYXNoIjoibVRmajh1UzY0MzVyRjA3dmhSLVEzQSIsInN1YiI6Imh0dHBzOi8vbG9naW4uc2FsZXNmb3JjZS5jb20vaWQvMDBEajAwMDAwMDI4Z0N1RUFJLzAwNTNaMDAwMDBLUGxwVlFBVCIsInpvbmVpbmZvIjoiR01UIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImFkZHJlc3MiOnt9LCJwcm9maWxlIjoiaHR0cHM6Ly9wa3NmLWRldi1lZC5teS5zYWxlc2ZvcmNlLmNvbS8wMDUzWjAwMDAwS1BscFZRQVQiLCJpc3MiOiJodHRwczovL215cGtzY29tbXVuaXR5LWRldmVsb3Blci1lZGl0aW9uLm5hMTAyLmZvcmNlLmNvbS9OVE9DdXN0b21lcnMiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJleHRlcm5hbHVzZXIxNTYxMTM0MTU0NTIyQGNvbXBhbnkuY29tIiwiZ2l2ZW5fbmFtZSI6IkVkd2FyZCIsImxvY2FsZSI6ImVuX1VTIiwibm9uY2UiOiIxMjM0IiwicGljdHVyZSI6Imh0dHBzOi8vbXlwa3Njb21tdW5pdHktZGV2ZWxvcGVyLWVkaXRpb24ubmExMDIuZm9yY2UuY29tL2ltZy91c2VycHJvZmlsZS9kZWZhdWx0X3Byb2ZpbGVfMjAwX3YyLnBuZyIsImF1ZCI6IjNNVkc5Zk10Q2tWNmVMaGNPTm05cUkxYjhNUXpDaHhBZ0doQXh0VmhrcGk2S2RNcXdPWUMwUzJNRjJQaUFMNEZHcmZuSHlJLnNIZDJkWnRwcDhkUlYsaHR0cHM6Ly9maW5zbS5hYWEuY29tIiwidXBkYXRlZF9hdCI6IjIwMTktMDYtMjFUMTY6MjI6MzRaIiwibmlja25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwibmFtZSI6IkVkd2FyZCBFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwicGhvbmVfbnVtYmVyIjpudWxsLCJleHAiOjE1NjEzODQ2MTcsImlhdCI6MTU2MTM4NDQ5NywiZmFtaWx5X25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwiZW1haWwiOiJlZHJ1c2hAYWFhY2Fyb2xpbmFzLmNvbSJ9.qNm5XwQYEkzLWKX2jUhvcYsqJ_S8SM29lR2m80Q6pIT9Ge25hvovqp6HS_l4UAQ2e2zPHWZoB1PuqskdrQYLEK3uHSmR5U2HqtYVNCMwOCReYoMyqNJFNRiF3_zqBPcT-GFGEGd7SyrVhERCyrw9aG5-nM14sU35d9Y4e4H0GehjBo3Iki12DcLsZGwlmpsm5_6MCFK8V4kH0fMWSALKAXoSWwKDvqrJ4jJ2EptRnTHCB3YoOYqy2-tQCVruHP4sAP75J-D6kTTxmtzNa_Ka9AGx0FUEEpYqJqgipZYa9_U_M9MIaO-8UPPUkSKeHANhlHISn77WMsr2Vv9z1HghIqpDjj1LUZ02J8ksx2QdT3zlJzGb35N-0xq_qvDfYL8zYfndJjmI0eH7WSZUWc_5cngFOMxy0pkmeiROAIFyj7_v_i5_75QXTvKfcPYLZHxOB7DDJkqaYkosdsmSPCtLK0PkowMXM5Hr8Lhc5DD4dzBnTrzDxYppUpMqO_fCI71NHXR0SYaaxqZI4Aiiult27wN1oIkJSP8M6hAyLG1TYEHuEXVRbT2fMTJUPW6MC8wLzFSrQxbgdo30UxrBUTccZOxhE-WYSMcB46aU2yplX8Y89s_OlKjEgKskxcUnzftTOAvdf9i9_gc9GwPPAsL4q-pHvvOX5SmwPPXdJOzV6LU&state=FINSM&scope=refresh_token+openid&token_type=Bearer";
            string source = @"finsmaaa://sflogin#access_token=00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf&refresh_token=5Aep861E3ECfhV22nYuzbKoDWYBlo3oTokN1ATS_.dGHyLC5GGMTkXjKFtThUIi3sVuy724DNCb3JDbiAymUtqb&instance_url=https%3A%2F%2Fpksf-dev-ed.my.salesforce.com&id=https%3A%2F%2Flogin.salesforce.com%2Fid%2F00Dj0000å0028gCuEAI%2F0053Z00000KPlpVQAT&issued_at=1561384497680&signature=ed1%2F6R2Z%2BtBHwLdlXuUcb3SD0yTKkDdV0J6NL%2FnlpIM%3D&sfdc_community_url=https%3A%2F%2Fmypksåcommunity-developer-edition.na102.force.com%2FNTOCustomers&sfdc_community_id=0DB3Z0000008QM8WAM&id_token=eyJraWQiOiIyMjAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdF9oYXNoIjoibVRmajh1UzY0MzVyRjA3dmhSLVEzQSIsInN1YiI6Imh0dHBzOi8vbG9naW4uc2FsZXNmb3JjZS5jb20vaWQvMDBEajAwMDAwMDI4Z0N1RUFJLzAwNTNaMDAwMDBLUGxwVlFBVCIsInpvbmVpbmZvIjoiR01UIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImFkZHJlc3MiOnt9LCJwcm9maWxlIjoiaHR0cHM6Ly9wa3NmLWRldi1lZC5teS5zYWxlc2ZvcmNlLmNvbS8wMDUzWjAwMDAwS1BscFZRQVQiLCJpc3MiOiJodHRwczovL215cGtzY29tbXVuaXR5LWRldmVsb3Blci1lZGl0aW9uLm5hMTAyLmZvcmNlLmNvbS9OVE9DdXN0b21lcnMiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJleHRlcm5hbHVzZXIxNTYxMTM0MTU0NTIyQGNvbXBhbnkuY29tIiwiZ2l2ZW5fbmFtZSI6IkVkd2FyZCIsImxvY2FsZSI6ImVuX1VTIiwibm9uY2UiOiIxMjM0IiwicGljdHVyZSI6Imh0dHBzOi8vbXlwa3Njb21tdW5pdHktZGV2ZWxvcGVyLWVkaXRpb24ubmExMDIuZm9yY2UuY29tL2ltZy91c2VycHJvZmlsZS9kZWZhdWx0X3Byb2ZpbGVfMjAwX3YyLnBuZyIsImF1ZCI6IjNNVkc5Zk10Q2tWNmVMaGNPTm05cUkxYjhNUXpDaHhBZ0doQXh0VmhrcGk2S2RNcXdPWUMwUzJNRjJQaUFMNEZHcmZuSHlJLnNIZDJkWnRwcDhkUlYsaHR0cHM6Ly9maW5zbS5hYWEuY29tIiwidXBkYXRlZF9hdCI6IjIwMTktMDYtMjFUMTY6MjI6MzRaIiwibmlja25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwibmFtZSI6IkVkd2FyZCBFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwicGhvbmVfbnVtYmVyIjpudWxsLCJleHAiOjE1NjEzODQ2MTcsImlhdCI6MTU2MTM4NDQ5NywiZmFtaWx5X25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwiZW1haWwiOiJlZHJ1c2hAYWFhY2Fyb2xpbmFzLmNvbSJ9.qNm5XwQYEkzLWKX2jUhvcYsqJ_S8SM29lR2m80Q6pIT9Ge25hvovqp6HS_l4UAQ2e2zPHWZoB1PuqskdrQYLEK3uHSmR5U2HqtYVNCMwOCReYoMyqNJFNRiF3_zqBPcT-GFGEGd7SyrVhERCyrw9aG5-nM14sU35d9Y4e4H0GehjBo3Iki12DcLsZGwlmpsm5_6MCFK8V4kH0fMWSALKAXoSWwKDvqrJ4jJ2EptRnTHCB3YoOYqy2-tQCVruHP4sAP75J-D6kTTxmtzNa_Ka9AGx0FUEEpYqJqgipZYa9_U_M9MIaO-8UPPUkSKeHANhlHISn77WMsr2Vv9z1HghIqpDjj1LUZ02J8ksx2QdT3zlJzGb35N-0xq_qvDfYL8zYfndJjmI0eH7WSZUWc_5cngFOMxy0pkmeiROAIFyj7_v_i5_75QXTvKfcPYLZHxOB7DDJkqaYkosdsmSPCtLK0PkowMXM5Hr8Lhc5DD4dzBnTrzDxYppUpMqO_fCI71NHXR0SYaaxqZI4Aiiult27wN1oIkJSP8M6hAyLG1TYEHuEXVRbT2fMTJUPW6MC8wLzFSrQxbgdo30UxrBUTccZOxhE-WYSMcB46aU2yplX8Y89s_OlKjEgKskxcUnzftTOAvdf9i9_gc9GwPPAsL4q-pHvvOX5SmwPPXdJOzV6LU&state=FINSM&scope=refresh_token+openid&token_type=Bearer";

            AccessTokenResponseManager _parser = new AccessTokenResponseManager(regularExpression);


            bool parseResult = _parser.Validate(source);

        }

        public static void ErrorParser()
        {

            AccessTokenErrorResponse _parser = new AccessTokenErrorResponse();
            string source = "\u003Chead>\u003C/head>\u003Cbody>\u003Cpre style=\"word-wrap: break-word; white-space: pre-wrap;\">error=redirect_uri_mismatch&amp;error_description=redirect_uri%20must%20match%20configuration\u003C/pre>\u003C/body>";

            
            bool parseResult = _parser.Validate(source);
        }
    }
}
